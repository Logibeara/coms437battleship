using UnityEngine;
using System.Collections;

public class EnemyMainGun : MonoBehaviour {

	private float degreesPerTick = 1;
	private float shotsPerTick = 1;
	private FSM_State fsm_state;
	private Battleship playerBattleship;
	public ExBattleship exBattleship;
	
	public EnemyBattleship enemyBattleship;
	private int gunCharge = 0;
	private int gunFireThreshold = 0;
	private int numWorkers = 2;
	private float hitPercentage = 0.10f;
	private Quaternion defaultOrientation;
	private int workTick = 0;
	private Quaternion tempLocalRot;
	//hack for testing
	bool enemyShipExists = true;

	GunExplosionEffect effect;
	private Vector3 getNearestEnemy()
	{
		if (exBattleship == null) {
			GameObject gameobj = GameObject.FindGameObjectWithTag("Battleship3d");
			if(gameobj != null)
			{
				exBattleship = gameobj.GetComponent<ExBattleship>();
			}
			return Vector3.zero;

		}
		return exBattleship.transform.position;
	}

	private enum FSM_State
	{
		NoTargetAvailable,
		AimingTowardsTarget,
		Charging,
		Firing,
		ShipDead,
	}
	// Use this for initialization
	void Start () {
		defaultOrientation = transform.localRotation;
		tempLocalRot = defaultOrientation;

		effect = this.GetComponent<GunExplosionEffect> ();

		gunFireThreshold = Random.Range (0, 3000);
	}

	void Awake()
	{
		playerBattleship = (GameObject.FindWithTag ("PlayerBattleship") as GameObject).GetComponent(typeof(Battleship)) as Battleship;
		enemyBattleship = (GameObject.FindWithTag ("EnemyBattleship") as GameObject).GetComponent(typeof(EnemyBattleship)) as EnemyBattleship;
	}
	
	// Update is called once per frame
	void Update () {
		//simultate x workers doing work
		for(int i = 0; i < numWorkers; i++)
		{
			doWork();
		}
		//consume all available work ticks
		while(workTick > 0)
		{
			workTick --;
			BehaviorFSMDoOne();
		}

		if (enemyBattleship.fsm_state != EnemyBattleship.FSM_State.Alive) 
		{
			fsm_state = FSM_State.ShipDead;
		}
	}


	private void fire()
	{
		//hit center for now
		if (playerBattleship != null) {
			if(Random.Range(0.0f,1.0f) <= hitPercentage)
			{
				playerBattleship.ProjectileHit (
				new Vector2 (playerBattleship.transform.position.x-2.5f, playerBattleship.transform.position.y-2.5f));
				exBattleship.DoDamage();
			}
			else
			{
				exBattleship.Miss();
			}
			fsm_state = FSM_State.AimingTowardsTarget;
			effect.Burst();
		} else {
			playerBattleship = (GameObject.FindGameObjectWithTag ("PlayerBattleship") as GameObject).GetComponent<Battleship> ();

		}
	}
	public bool doWork()
	{
		//only do work if the position is within one unit of this gun

		workTick ++;
		return true;


		
	}

	void BehaviorFSMDoOne()
	{
		
		Quaternion currentOrientation = tempLocalRot;

		switch (fsm_state)
		{
		case(FSM_State.NoTargetAvailable):
		
			//go to defaultPosition
			
			
			if(!enemyShipExists)
			{
				
				transform.rotation = Quaternion.RotateTowards(currentOrientation,defaultOrientation,degreesPerTick);
			}
			else
			{
				fsm_state = FSM_State.AimingTowardsTarget;
			}
			
			break;
		case(FSM_State.AimingTowardsTarget):
		case(FSM_State.Charging):
			Vector3 enemyPosition = getNearestEnemy();
			
			Quaternion rot = 
				Quaternion.FromToRotation(currentOrientation  * new Vector3(0,0,-1),  enemyPosition- this.transform.position) * currentOrientation;
			
			
			tempLocalRot = Quaternion.RotateTowards(currentOrientation,rot,degreesPerTick);// * defaultOrientation;

			transform.rotation = tempLocalRot;
			if(Quaternion.Angle( transform.rotation , rot ) < 1.0)
			{
				fsm_state = FSM_State.Charging;
			}
			
			gunCharge ++;
			if(gunCharge >= gunFireThreshold)
			{
				fsm_state =  FSM_State.Firing;
			}

			break;


		case(FSM_State.Firing):
			fire();
			
			gunCharge = 0;

			break;
		case(FSM_State.ShipDead):
			//do nothing
			break;
			
		}
	}
}
