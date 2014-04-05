using UnityEngine;
using System.Collections;

enum CrewMemberStatus
{
	IDLE_WANDER,
	TIRED,
	PERFORM_JOB
}

public class CrewMember : MonoBehaviour {

	private float wanderCircleRadius = 1f;
	private float wanderAngle;
	private float velocityMax = 2f;
	private Random rand = new Random();

	//The current major goal position of the crew member (usually defined
	// by the CrewMemberStatus and the active job)
	private Vector2 target;

	//The current short-term target of the crew member. Factors (ordered from highest to lowest precedence):
	//  -The proximity to the nearest finger currently in contact with the screen (if any)
	//  -The direction of the next waypoint towards the major target, as selected by a pathfinding algorithm
	//  -The force applied by any Station whose sphere of influence this crew member currently occupies
	private Vector2 intermediateTarget;

	private CrewMemberStatus status;
	private Station activeJob;

//	public Vector2 Position
//	{ 
//		get { return new Vector2 (transform.position.x, transform.position.y); }
//		set { transform.position = new Vector3(value.x, value.y, 0); }
//	}
	public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

	// Use this for initialization
	void Start () {
		status = CrewMemberStatus.IDLE_WANDER;
		wanderAngle = Random.value * 360;
	}
	
	// Update is called once per frame
	void Update () {
		//Update the intermediate target
		switch(status)
		{
		case CrewMemberStatus.IDLE_WANDER:
			if(rigidbody2D.velocity.magnitude <= velocityMax)
			{
				Vector2 forwardVec = 2 * (new Vector2(transform.forward.x, transform.forward.y));
				Vector2 circlePos = new Vector2(Position.x,  Position.y) + forwardVec;
				wanderAngle += (Random.value * 30) - 15;

				Vector2 targetPos = circlePos + new Vector2(Mathf.Cos(wanderAngle * Mathf.Deg2Rad), Mathf.Sin(wanderAngle * Mathf.Deg2Rad));

				//Apply linear force
				rigidbody2D.AddForce(1 * new Vector2(targetPos.x - Position.x, targetPos.y - Position.y));

				Vector3 cross = Vector3.Cross(new Vector3(rigidbody2D.velocity.normalized.x, rigidbody2D.velocity.normalized.y, 0), rigidbody2D.transform.up.normalized);
				rigidbody2D.AddTorque((cross.z < 0) ? .4f : -.4f);

			}

			break;

		case CrewMemberStatus.PERFORM_JOB:
			break;

		case CrewMemberStatus.TIRED:
			break;
		}

		if(rigidbody2D.velocity.magnitude > velocityMax)
		{
			rigidbody2D.velocity = rigidbody2D.velocity.normalized *  velocityMax;
		}
	}

	//After updating the intermediate target, perform wall avoidance
}
