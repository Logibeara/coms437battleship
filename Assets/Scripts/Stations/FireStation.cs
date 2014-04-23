using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireStation : MonoBehaviour, Station {
	private int health;
	List<CrewMember> crewList;
	private int workTick;

	// Use this for initialization
	void Start ()
	{
		health = 100;
		crewList = new List<CrewMember> ();
		workTick = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		while(workTick > 0)
		{
			workTick --;
			//DO WORK!!
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
	
	Vector2 getTarget(CrewMember crewIn);
	
	bool doDamage(int dammageDone)
	{
		health -= dammageDone;
		if (health >= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	/// <summary>
	/// return true of work was done	/// </summary>
	/// <returns>The work.</returns>
	/// <param name="position">Position.</param>
	bool doWork(Vector2 position) //does work based on posistion if possible
	{
		//TODO checi if near a fire
		//if so work++
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
