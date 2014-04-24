using UnityEngine;
using System.Collections;

public class EnemyMainGun : MonoBehaviour {

	private float degreesPerTick = 1;
	private float shotsPerTick = 1;
	private FSM_State fsm_state;
	private Battleship playerBattleship;
	public ExBattleship exBattleship;
	private int gunCharge = 0;
	private int gunFireThreshold = 2000;
	private int numWorkers = 2;
	
	private Quaternion defaultOrientation;
	private int workTick = 0;
	private Quaternion tempLocalRot;
	//hack for testing
	bool enemyShipExists = true;

	GunExplosionEffect effect;
	private Vector3 getNearestEnemy()
	{
		return exBattleship.transform.position;
	}

	private enum FSM_State
	{
		NoTargetAvailable,
		AimingTowardsTarget,
		Charging,
		Firing,
	}
	// Use this for initialization
	void Start () {
		playerBattleship = (GameObject.FindGameObjectWithTag ("PlayerBattleship") as GameObject).GetComponent<Battleship> ();
		defaultOrientation = transform.localRotation;
		tempLocalRot = defaultOrientation;

		effect = this.GetComponent<GunExplosionEffect> ();
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
	}


	private void fire()
	{
		//hit center for now
		if (playerBattleship != null) {
				playerBattleship.ProjectileHit (
		new Vector2 (playerBattleship.transform.position.x-2.5f, playerBattleship.transform.position.y-2.5f));
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
			
		}
	}
}
