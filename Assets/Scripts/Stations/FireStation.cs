using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireStation : MonoBehaviour, Station {
	private int workTick;
	List<Fire> fireList;
	public Battleship battleShip;

	// Use this for initialization
	void Start ()
	{
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

	public Vector2 getTarget(CrewMember crewIn)
	{
		//TODO return location of closest fire when we have that information
		if(fireList == null)
		{
			fireList = battleShip.Fires;
		}
		if(fireList.Count > 0)
		{
			Fire nearest = fireList[0];
			float distance = Vector2.Distance (nearest.transform.position, crewIn.rigidbody2D.transform.position);
			float checkDist;
			fireList.RemoveAll(item => item == null);
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
		else
		{
			return this.transform.position;
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
		foreach(Fire fire in fireList)
		{
			if(Vector2.Distance(fire.transform.position, position) < 0.6f && Vector2.Distance(fire.transform.position, position) > -0.6f)
			{
				fire.fightFire();
				worker.Tiredness++;
				return true;
			}
		}
		Debug.Log("ERROR: Did work on fire from far away"); 
		return false;
	}
}
