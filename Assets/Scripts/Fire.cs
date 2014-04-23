using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	float health;
	int workTicker;
	List<Fire> fireList;

	float timeSinceLastSpread;

	public List<Fire> FireList
	{
		get { return fireList; }
		set { fireList = value; }
	}

	public float Health
	{
		get{ return health;	}
		set{ health = value; }
	}

	// Use this for initialization
	void Start ()
	{
		health = 1000;
		workTicker = 0;
		timeSinceLastSpread = 0;
	}

	public void doWork()
	{
		workTicker++;
	}
	
	// Update is called once per frame
	void Update ()
	{
		health -= workTicker * Time.deltaTime;
		if(health <= 0)
		{
			destroy();
		}

		if(timeSinceLastSpread > 5 && Random.value < 0.01f)
		{
			//Attempt to spawn a new adjacent fire
			//float xOffset
		}
	}

	private void destroy()
	{
		//remove from the fire list on the battle ship
		if(fireList != null)
		{
			fireList.Remove (this);
		}
		else
		{
			throw new System.ArgumentNullException("fireList is null!!!!");
		}
	}
}