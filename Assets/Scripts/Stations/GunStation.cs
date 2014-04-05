using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.ArrayList;

public class GunStation : MonoBehaviour, Station {
	private int health;
	List<CrewMember> crewList;

	// Use this for initialization
	void Start () {
		health = 100;
		crewList = new List<CrewMember> ();
	}
	
	// Update is called once per frame
	void Update () {
	
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

	public bool doDammage(int dammageDone)
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
