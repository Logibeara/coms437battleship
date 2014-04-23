using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	float currentHealth;
	float maxHealth;

	int workTicker;
	List<Fire> fireList;

	private Vector3 defaultScale;

	private float timeSinceLastSpread;

	public List<Fire> FireList
	{
		get { return fireList; }
		set { fireList = value; }
	}

	public float Health
	{
		get{ return currentHealth;	}
		set{ currentHealth = value; }
	}

	// Use this for initialization
	void Start ()
	{
		maxHealth = 10;
		currentHealth = maxHealth;
		workTicker = 0;
		timeSinceLastSpread = 0;
		defaultScale = this.transform.localScale;
	}

	public void fightFire()
	{
		workTicker++;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentHealth -= (float)workTicker * Time.deltaTime;

		this.transform.localScale = defaultScale * currentHealth / maxHealth;

		timeSinceLastSpread += Time.deltaTime;
		if(currentHealth <= 0)
		{
			extinguish();
		}

		if(timeSinceLastSpread > 12 && Random.value < 0.0005f)
		{
			Vector3 newPos = this.transform.position;

			//Offset the position by a small, random amount
			newPos.x += Random.value * .2f - .1f;
			newPos.y += Random.value * .2f - .1f;

			//Attempt to spawn a new fire
			fireList.Add(Instantiate(this.gameObject, newPos, this.transform.rotation) as Fire);
			timeSinceLastSpread = 0;
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