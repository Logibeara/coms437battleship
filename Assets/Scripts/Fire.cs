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
		health = 10;
		workTicker = 0;
		timeSinceLastSpread = 0;
	}

	public void fightFire()
	{
		workTicker++;
	}
	
	// Update is called once per frame
	void Update ()
	{
		health -= (float)workTicker * Time.deltaTime;
		if(health <= 0)
		{
			extinguish();
		}

		if(timeSinceLastSpread > 5 && Random.value < 0.01f)
		{
			//Attempt to spawn a new adjacent fire
			//float xOffset
		}

		workTicker = 0;
	}

	private void extinguish()
	{
		//remove from the fire list on the battle ship
		if(fireList != null)
		{
			fireList.Remove (this);
			Destroy(this.gameObject);

		}
		else
		{
			throw new System.ArgumentNullException("fireList is null!!!!");
		}
	}
}