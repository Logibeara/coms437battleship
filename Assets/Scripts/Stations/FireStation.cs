using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireStation : MonoBehaviour, Station {
	private int health;
	List<CrewMember> crewList;
	private int workTick;
	List<Fire> fireList;
	public Battleship battleShip;

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

	}

	public void setFireList(List<Fire> fireListIn)
	{
		fireList = fireListIn;
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
	
	public Vector2 getTarget(CrewMember crewIn)
	{
		//TODO return location of closest fire when we have that information
		Fire nearest = fireList[0];
		float distance = Vector2.Distance (nearest.transform.position, crewIn.rigidbody2D.transform.position);
		float checkDist;
		foreach(Fire fire in fireList)
		{
			checkDist = Vector2.Distance(fire.transform.position, crewIn.rigidbody2D.transform.position);
			if(checkDist < distance)
			{
				nearest = fire;
				distance = checkDist;
			}
		}

		return nearest.transform.position;
	}
	
	public bool doDamage(int dammageDone)
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
	public bool doWork(Vector2 position) //does work based on posistion if possible
	{
		//TODO check if near a fire
		//if so work++
		bool ret = false;
		foreach(Fire fire in fireList)
		{
			if(Vector2.Distance(fire.transform.position, position) < 0.6f && Vector2.Distance(fire.transform.position, position) > -0.6f)
			{
				fire.doWork();
				ret = true;
				break;
			}
		}
		return true;
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
