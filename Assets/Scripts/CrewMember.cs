using UnityEngine;
using System.Collections;

enum CrewMemberStatus
{
	IDLE_WANDER,
	TIRED,
	PERFORM_JOB
}

public class CrewMember : MonoBehaviour {

	//private Vector2 position;

	//The current major goal position of the crew member (usually defined
	// by the CrewMemberStatus and the active job)
	private Vector2 target;

	//The current short-term target of the crew member. Factors (ordered from highest to lowest precedence):
	//  -The proximity to the nearest finger currently in contact with the screen (if any)
	//  -The direction of the next waypoint towards the major target, as selected by a pathfinding algorithm
	//  -The force applied by any Station whose sphere of influence this crew member currently occupies
	private Vector2 intermediateTarget;

	private Station activeJob;

//	public Vector2 Position
//	{ 
//		get { return new Vector2 (transform.position.x, transform.position.y); }
//		set { transform.position = new Vector3(value.x, value.y, 0); }
//	}
	public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

	// Use this for initialization
	void Start () {
		//position = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		//switch(
	}
}
