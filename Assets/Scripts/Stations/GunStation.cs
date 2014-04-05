using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.ArrayList;

public class GunStation : MonoBehaviour, Station {
	int health;
	List<CrewMember> crewList;

	// Use this for initialization
	void Start () {
		health = 0;
		crewList = new List<CrewMember> ();
	}
	
	// Update is called once per frame
	void Update () {
	
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
