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
		worker.Tiredness -= (int)(Random.value * 10);

		if(worker.Tiredness <= 0)
		{
			worker.ResumeLastKnownJob();
		}

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
