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
	private int tired;

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
	public GameObject barracks;
	public Station barracksScript;

	/// <summary>
	/// give this crew member a job
	/// </summary>
	/// <param name="station">Station.</param>
	public void SetStation(Station station)
	{
		if (activeJob != station) {
			activeJob = station;
			status = CrewMemberStatus.PERFORM_JOB;
		}
	}
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
		tired = 0;
		barracksScript = barracks.GetComponent (typeof(Station)) as Station;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 targetPos; //temp variable

		//float aggregateTorque = 0;
		Vector2 aggregateForce = Vector2.zero;

		//Update the intermediate target
		switch(status)
		{
		case CrewMemberStatus.IDLE_WANDER:
			if(rigidbody2D.velocity.magnitude <= velocityMax)
			{

				//Generate wander target to follow
				Vector2 forwardVec = 4 * (new Vector2(transform.forward.x, transform.forward.y));
				Vector2 circlePos = new Vector2(Position.x,  Position.y) + forwardVec;
				wanderAngle += (Random.value * 30) - 15;
				targetPos = circlePos + 0.05f * new Vector2(Mathf.Cos(wanderAngle * Mathf.Deg2Rad), Mathf.Sin(wanderAngle * Mathf.Deg2Rad));

				ApplyTowardsTarget(targetPos, ref aggregateForce);
			}
			if(Random.value * 1000000 + 1000 <= tired)
			{
				status = CrewMemberStatus.TIRED;
				//tired = 0;
			}

			break;

		case CrewMemberStatus.PERFORM_JOB:
			/*if(activeJob == null)
			{
				status = CrewMemberStatus.IDLE_WANDER;
			}
			else if(Vector2.Distance(rigidbody2D.transform.position, activeJob.getTarget(this)) < .3f)
			{
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0;
				activeJob.doWork();
			}
			else
			{
				ApplyTowardsTarget(activeJob.getTarget(this), ref aggregateForce);
			}*/
			bool didWork = activeJob.doWork(this.transform.position);
			if(didWork)
			{
				//don't move
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0;
			}
			else
			{
				ApplyTowardsTarget(activeJob.getTarget(this), ref aggregateForce);
			}
			if(Random.value * 100000 + 1000 <= tired)
			{
				status = CrewMemberStatus.TIRED;
				//tired = 0;
			}
			break;

		case CrewMemberStatus.TIRED:
			targetPos = barracksScript.getTarget(this);

			//If we're at the barracks
			if(Vector2.Distance(rigidbody2D.transform.position, targetPos) < .4f)
			{
				//Stop animation
				(gameObject.GetComponent(typeof(Animator)) as Animator).enabled = false;

				tired -= (int)(Random.value * 10);
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0;
			}
			else
			{
				//Go to the barracks
				if(rigidbody2D.velocity.magnitude <= velocityMax)
				{
					ApplyTowardsTarget(targetPos, ref aggregateForce);
					
					//Apply linear force
					//				rigidbody2D.AddForce(1 * new Vector2(targetPos.x - Position.x, targetPos.y - Position.y));
					//				
					//				Vector3 cross = Vector3.Cross(new Vector3(rigidbody2D.velocity.normalized.x, rigidbody2D.velocity.normalized.y, 0), rigidbody2D.transform.up.normalized);
					//				rigidbody2D.AddTorque((cross.z < 0) ? .2f : -.2f);
				}
			}
			if(tired <= 0)
			{
				status = CrewMemberStatus.PERFORM_JOB;
				(gameObject.GetComponent(typeof(Animator)) as Animator).enabled = true;
			}
//			if(Random.value * 1000 <= tired)
//			{
//				status = CrewMemberStatus.PERFORM_JOB;
//				tired = 0;
//			}
			break;
		}

		if(rigidbody2D.velocity.magnitude > velocityMax)
		{
			rigidbody2D.velocity = rigidbody2D.velocity.normalized *  velocityMax;
		}
		if(rigidbody2D.angularVelocity > .01f)
		{
			rigidbody2D.angularVelocity = rigidbody2D.angularVelocity *  .01f;
		}

		//After updating the force, perform wall avoidance
		//Physics2D.Raycast(
		Debug.DrawRay (rigidbody2D.transform.position + rigidbody2D.transform.up, aggregateForce.normalized);


		//Apply linear force and torque
		rigidbody2D.AddForce(aggregateForce);
		if(aggregateForce.magnitude > .01)
		{
			Vector3 cross = Vector3.Cross(new Vector3(rigidbody2D.velocity.normalized.x, rigidbody2D.velocity.normalized.y, 0), rigidbody2D.transform.up.normalized);
			rigidbody2D.AddTorque((cross.z < 0) ? .5f : -.5f);
		}

		tired++;
	}

	private void ApplyTowardsTarget(Vector2 target, ref Vector2 force)
	{
		force += new Vector2 (target.x - Position.x, target.y - Position.y).normalized;
	}
}
