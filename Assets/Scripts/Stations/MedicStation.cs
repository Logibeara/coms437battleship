using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedicStation : MonoBehaviour, Station {

	List<CrewMember> crewList;
	public Battleship battleShip;
	List<CrewMember> incapCrewList;

	// Use this for initialization
	void Start ()
	{
		crewList = new List<CrewMember>();
		incapCrewList = new List<CrewMember>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach(CrewMember crew in incapCrewList)
		{
			//TODO if healed then remove from list
			//if(crew.
		}

		foreach(CrewMember crew in crewList)
		{
			//TODO if incapped then add to list
		}
	}

	public void setCrewList(List<CrewMember> fireListIn)
	{
		crewList = fireListIn;
	}

	public Vector2 getTarget(CrewMember crewIn)
	{
		if(crewList == null)
		{
			//TODO make a ships crew accessable
			//crewList = battleShip.Crew;
		}

		//TODO
		return crewIn.transform.position;
	}
	
	/// <summary>
	/// return true of work was done	/// </summary>
	/// <returns>The work.</returns>
	/// <param name="position">Position.</param>
	public bool doWork(CrewMember worker) //does work based on position if possible
	{
		//TODO
		return true;
	}
}
