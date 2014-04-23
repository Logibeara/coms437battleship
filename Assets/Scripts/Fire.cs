using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	float health;
	int workTicker;

	// Use this for initialization
	void Start ()
	{
		health = 1000;
		workTicker = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		health -= workTicker * Time.deltaTime;
		if(health <= 0)
		{
			destroy();
		}
	}

	public int Health
	{
		get
		{
			return health;
		}
		set
		{
			health = value;
		}
	}

	private void destroy()
	{
		//remove from the fire list on the battle ship

	}
}