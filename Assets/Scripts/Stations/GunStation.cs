using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using System.Collections.ArrayList;

public class GunStation : MonoBehaviour, Station {
	private int health;
	List<CrewMember> crewList;
	private int workTick = 0;
	private Quaternion defaultOrientation;
	private Animator anim;					// Reference to the Animator component.
	public int shootSemaphore = 0; //counter to synchronize shots with 3d representation

	//queue for recycling bullet instances
	private ExplosiveRound[] bulletQueue;
	private int queueLength = 10;
	private int queueIndex = 0;
	// Use this for initialization
	void Start () {

		health = 100;
		anim = transform.root.gameObject.GetComponent<Animator> ();
		fsm_state = FSM_State.NoTargetAvailable;
		crewList = new List<CrewMember> ();
		defaultOrientation = transform.localRotation;

		bulletQueue =new ExplosiveRound[queueLength];
		Object bObj = Resources.Load ("Prefabs/ExplosiveRound");
		for (int i = 0; i<queueLength; i++) 
		{
			
			bulletQueue[i] = (Instantiate (bObj) as GameObject).GetComponent(typeof(ExplosiveRound)) as ExplosiveRound;
		
			//send off screen
			bulletQueue[i].transform.position = new Vector3(100,100,100);
		}

	}

	ExplosiveRound getNextBullet()
	{
		ExplosiveRound ret =  bulletQueue [queueIndex];
		queueIndex ++;
		if (queueIndex >= queueLength) 
		{
			queueIndex = 0;
		}
		return ret;
	}
	// Update is called once per frame
	void FixedUpdate () {
	
		//consume all available work ticks
		while(workTick > 0)
		{
			workTick --;
			BehaviorFSMDoOne();
		}
	}
	public enum FSM_State
	{
		NoTargetAvailable,
		AimingTowardsTarget,
		Charging,
		Firing,
	}

	private float degreesPerTick = 1;
	private float shotsPerTick = 1;
	public FSM_State fsm_state;
	private int gunCharge = 0;
	private int gunFireThreshold = 100;

	//hack for testing
	bool enemyShipExists = true;

	private void fire()
	{
		anim.SetTrigger ("Shoot");


		ExplosiveRound bulletInstance1 = getNextBullet ();
		ExplosiveRound bulletInstance2 = getNextBullet ();
		ExplosiveRound bulletInstance3 = getNextBullet ();

		
		Transform[] barrelPos = {
			transform.FindChild("firelocation1"),
			transform.FindChild("firelocation2"),
			transform.FindChild("firelocation3")};



		bulletInstance1.transform.rotation = barrelPos [0].transform.rotation;
		bulletInstance2.transform.rotation = barrelPos[1].transform.rotation;
		bulletInstance3.transform.rotation = barrelPos[2].transform.rotation;

		
		bulletInstance1.transform.position = barrelPos [0].position;
		bulletInstance2.transform.position = barrelPos [1].position;
		bulletInstance3.transform.position = barrelPos [2].position;


		Vector3 fwd = this.transform.localRotation * this.transform.forward;
		float speed = 10.0f;
		//exMainGun1.transform.rotation = UnityEngine.Quaternion.Euler(0f, -mainGun1.transform.eulerAngles.z, 0f);
		Vector3 direction = getNearestEnemy () - this.transform.position;
		direction.Normalize ();

		//getenemyPosition- this.transform.position;

		bulletInstance1.rigidbody2D.velocity =  speed * new Vector2(direction.x,direction.y );

		bulletInstance2.rigidbody2D.velocity = speed *  new Vector2(direction.x,direction.y);
		bulletInstance3.rigidbody2D.velocity = speed *  new Vector2(direction.x,direction.y );
		shootSemaphore++;

	}
	GameObject enemy;
	GameObject battleship3d;

	private Vector3 getNearestEnemy()
	{

		if (enemy == null || battleship3d == null) {
			enemy = GameObject.FindWithTag ("EnemyBattleship");
			battleship3d = GameObject.FindWithTag ("Battleship3d");

			return new Vector3(-10, 0, 0);

		} else {
			Vector3 pos = enemy.transform.position - battleship3d.transform.position;
			if(enemy.GetComponent<EnemyBattleship>().fsm_state != EnemyBattleship.FSM_State.Alive)
			{
				enemyShipExists = false;
			}
			else
			{
				enemyShipExists = true;
			}
			return 10 * new Vector2(pos.x, pos.z);
		}

	}
	void BehaviorFSMDoOne()
	{
		
		Quaternion currentOrientation = transform.localRotation;
		Vector3 enemyPosition = getNearestEnemy();
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



			if(enemyShipExists)
			{
				Quaternion rot = Quaternion.FromToRotation(currentOrientation * new Vector3(0,-1,0),  enemyPosition- this.transform.position) * currentOrientation;
			
	
				if(Quaternion.Angle( transform.localRotation , rot ) < .05)
				{
					fsm_state = FSM_State.Charging;
				}

				transform.localRotation = Quaternion.RotateTowards(currentOrientation,rot,degreesPerTick);// * defaultOrientation;


			
				gunCharge ++;
				if(gunCharge >= gunFireThreshold && fsm_state == FSM_State.Charging)
				{
					gunCharge = 0;
					fsm_state = FSM_State.Firing;
				
				}
			}else
			{
				
				fsm_state = FSM_State.NoTargetAvailable;
			}
			
			break;
		case(FSM_State.Firing):
			fire();
			
			fsm_state = FSM_State.AimingTowardsTarget;
			break;
		}
	}


	public bool doWork(CrewMember worker)
	{
		Vector2 position = new Vector2(worker.transform.position.x, worker.transform.position.y);
		//only do work if the position is within one unit of this gun
		if((new Vector2(this.transform.position.x, this.transform.position.y) - position).magnitude < 1)
		{
			worker.Tiredness++;
			workTick ++;
			return true;
		}

		return false;

	}
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		CrewMember crewMember = other.GetComponent (typeof(CrewMember)) as CrewMember;
		if(crewMember != null)
		{
			crewMember.SetStation(this);
		}
	}
	public int Health{ 
		get
		{
			return health;
		}
		set
		{
			health = value;
		}
	}

	public bool doDamage(int damageDone)
	{
		health -= damageDone;
		if (health >= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void doWork()
	{
		//TODO
		return;
	}

	public Vector2 getTarget(CrewMember crewIn)
	{
		return this.transform.position;
	}

	public void addCrew(CrewMember crewIn)
	{
		if (crewIn != null)
		{
			crewList.Add(crewIn);
		}
	}
	public void removeCrew(CrewMember crewIn)
	{
		if(crewList.Contains(crewIn));
		{
			crewList.Remove(crewIn);
		}
	}
}
