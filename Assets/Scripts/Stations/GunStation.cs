using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.ArrayList;

public class GunStation : MonoBehaviour, Station {
	private int health;
	List<CrewMember> crewList;
	private int workTick = 0;
	private Quaternion defaultOrientation;
	// Use this for initialization
	void Start () {
		health = 100;
		fsm_state = FSM_State.NoTargetAvailable;
		crewList = new List<CrewMember> ();
		defaultOrientation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
	
		//consume all available work ticks
		while(workTick > 0)
		{
			workTick --;
			BehaviorFSMDoOne();
		}
	}
	private enum FSM_State
	{
		NoTargetAvailable,
		AimingTowardsTarget,
		Firing,
	}

	private float degreesPerTick = 1;
	private float shotsPerTick = 1;
	private FSM_State fsm_state;
	private int gunCharge = 0;
	private int gunFireThreshold = 6;

	//hack for testing
	bool enemyShipExists = true;

	private Vector3 getNearestEnemy()
	{
		//TODO
		//return directly left
		return new Vector3(-10, 0, 0);
	}
	void BehaviorFSMDoOne()
	{
		
		Quaternion currentOrientation = transform.localRotation;

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
			Vector3 enemyPosition = getNearestEnemy();

			Quaternion rot = Quaternion.FromToRotation(currentOrientation * new Vector3(0,-1,0),  enemyPosition- this.transform.position) * currentOrientation;
		
			transform.localRotation = Quaternion.RotateTowards(currentOrientation,rot,degreesPerTick);// * defaultOrientation;


			break;

		case(FSM_State.Firing):
			gunCharge ++;
			if(gunCharge >= gunFireThreshold)
			{
				gunCharge = 0;

				//FIRE!
			}


			break;

		}
	}


	public bool doWork(Vector2 position)
	{
		//only do work if the position is within one unit of this gun
		if((new Vector2(this.transform.position.x, this.transform.position.y) - position).magnitude < 1)
		{
			workTick ++;
			return true;

		}

		return false;

	}
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		CrewMember crewMember = other.GetComponent (typeof(CrewMember)) as CrewMember;
		crewMember.SetStation(this);
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
