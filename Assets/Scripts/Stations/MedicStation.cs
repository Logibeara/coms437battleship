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
			if(crew.Status != CrewMemberStatus.INCAPACITATED)
			{
				incapCrewList.Remove(crew);
			}
		}

		foreach(CrewMember crew in crewList)
		{
			//TODO if incapped then add to list
			if(crew.Status == CrewMemberStatus.INCAPACITATED && !incapCrewList.Contains(crew))
			{
				incapCrewList.Add(crew);
			}
		}
	}

	public void setCrewList(List<CrewMember> crewListIn)
	{
		crewList = crewListIn;
	}

	public Vector2 getTarget(CrewMember crewIn)
	{
		if(crewList == null)
		{
			crewList = battleShip.Crew;
		}
		if(incapCrewList.Count > 0)
		{
			CrewMember nearest = incapCrewList[0];
			float distance = Vector2.Distance (nearest.transform.position, crewIn.rigidbody2D.transform.position);
			float checkDist;
			incapCrewList.RemoveAll(item => item == null);
			foreach(CrewMember crew in incapCrewList)
			{
				checkDist = Vector2.Distance(crew.transform.position, crewIn.rigidbody2D.transform.position);
				if(checkDist < distance)
				{
					nearest = crew;
					distance = checkDist;
				}
			}
			return nearest.transform.position;
		}
		else
		{
			return crewIn.transform.position;
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent(typeof(CrewMember)) as CrewMember != null)
		{
			CrewMember crewMember = other.GetComponent (typeof(CrewMember)) as CrewMember;
			crewMember.SetStation(this);
		}
	}
	
	/// <summary>
	/// return true of work was done	/// </summary>
	/// <returns>The work.</returns>
	/// <param name="position">Position.</param>
	public bool doWork(CrewMember worker) //does work based on posistion if possible
	{
		Vector3 position = worker.transform.position;
		//TODO check if near a fire
		//if so work++
		foreach(CrewMember crew in incapCrewList)
		{
			if(Vector2.Distance(crew.transform.position, position) < 0.6f && Vector2.Distance(crew.transform.position, position) > -0.6f)
			{
				crew.heal();
				worker.Tiredness++;
				return true;
			}
		}
		return false;
	}
}
