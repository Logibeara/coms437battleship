using UnityEngine;
using System.Collections;

public class Barracks : MonoBehaviour, Station {

	// Use this for initialization
	void Start () {

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

	public void doWork()
	{
		return;
	}
	
	public Vector2 getTarget(CrewMember crewIn)
	{
		return this.transform.position;
	}
}
