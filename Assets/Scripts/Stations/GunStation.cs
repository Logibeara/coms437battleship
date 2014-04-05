using UnityEngine;
using System.Collections;

public class GunStation : MonoBehaviour, Station {
	int health;


	// Use this for initialization
	void Start () {
		health = 0;
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
}
