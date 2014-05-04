using UnityEngine;
using System.Collections;

public class Barracks : MonoBehaviour, Station {

	private int health;
	
	// Use this for initialization
	void Start () {
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool doWork(CrewMember worker)
	{
		//todo
		return true;
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

	public void doWork()
	{
		return;
	}
	
	public Vector2 getTarget(CrewMember crewIn)
	{
		return this.transform.position;
	}
	
	public void addCrew(CrewMember crewIn)
	{
		return;
	}
	public void removeCrew(CrewMember crewIn)
	{
		return;
	}
}
