using UnityEngine;
using System.Collections;

enum CrewMemberStatus
{
	IDLE_WANDER,
	TIRED,
	PERFORM_JOB
}

public class CrewMember : MonoBehaviour {
	
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
				Vector2 forwardVec = 100 * (new Vector2(transform.forward.x, transform.forward.y));
				Vector2 circlePos = new Vector2(Position.x,  Position.y) + forwardVec;
				wanderAngle += (Random.value * 20) - 10;
				targetPos = circlePos + 0.05f * new Vector2(Mathf.Cos(wanderAngle * Mathf.Deg2Rad), Mathf.Sin(wanderAngle * Mathf.Deg2Rad));

				ApplyTowardsTarget(targetPos, ref aggregateForce);
			}
			if(Random.value * 100000 + 1000 <= tired)
			{
				status = CrewMemberStatus.TIRED;
				//tired = 0;
			}

			break;

		case CrewMemberStatus.PERFORM_JOB:
			if(activeJob == null)
			{
				status = CrewMemberStatus.IDLE_WANDER;
			}
			else
			{
				ApplyTowardsTarget(activeJob.getTarget(this), ref aggregateForce);
			}
			break;

		case CrewMemberStatus.TIRED:
			targetPos = barracksScript.getTarget(this);

			//If we're at the barracks
			if(Vector2.Distance(rigidbody2D.transform.position, targetPos) < .3f)
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
				status = CrewMemberStatus.IDLE_WANDER;
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
//		RaycastHit2D lefthit = Physics2D.Raycast (rigidbody2D.transform.position + rigidbody2D.transform.up * .1f - rigidbody2D.transform.right*.2f, rigidbody2D.transform.up);
//		RaycastHit2D righthit = Physics2D.Raycast (rigidbody2D.transform.position + rigidbody2D.transform.up * .1f + rigidbody2D.transform.right*.2f, rigidbody2D.transform.up);
//		if (Vector2.Distance(lefthit.point, rigidbody2D.transform.position) < .2
//		    && Vector2.Distance(righthit.point, rigidbody2D.transform.position) < .4) {
//			print ("both");
//			ApplyTowardsTarget(-aggregateForce * .5f, ref aggregateForce);
//			ApplyTowardsTarget(-rigidbody2D.transform.right, ref aggregateForce);
//		}
//		else if (Vector2.Distance(lefthit.point, rigidbody2D.transform.position) < .4) {
//			print ("left");
//			ApplyTowardsTarget(-aggregateForce * .5f, ref aggregateForce);
//			ApplyTowardsTarget(rigidbody2D.transform.right, ref aggregateForce);
//		}
//		else if (Vector2.Distance(righthit.point, rigidbody2D.transform.position) < .4) {
//			print ("right");
//			ApplyTowardsTarget(-aggregateForce * .5f, ref aggregateForce);
//			ApplyTowardsTarget(-rigidbody2D.transform.right, ref aggregateForce);
//		}
		


		//Apply linear force and torque
		rigidbody2D.AddForce(aggregateForce);
		if(aggregateForce.magnitude > .01)
		{
			Vector3 cross = Vector3.Cross(new Vector3(rigidbody2D.velocity.normalized.x, rigidbody2D.velocity.normalized.y, 0), rigidbody2D.transform.up.normalized);
			rigidbody2D.AddTorque((cross.z < 0) ? .5f : -.5f);

//			//After updating the force, perform wall avoidance
//			RaycastHit2D lefthit = Physics2D.Raycast (rigidbody2D.transform.position - rigidbody2D.transform.right*.08f - rigidbody2D.transform.up*.1f, rigidbody2D.transform.up - rigidbody2D.transform.right * .1f);
//			RaycastHit2D righthit = Physics2D.Raycast (rigidbody2D.transform.position + rigidbody2D.transform.right*.08f - rigidbody2D.transform.up*.1f, rigidbody2D.transform.up + rigidbody2D.transform.right * .1f);
//
//			Debug.DrawRay (rigidbody2D.transform.position - rigidbody2D.transform.right*.08f - rigidbody2D.transform.up*.1f, rigidbody2D.transform.up /*- rigidbody2D.transform.right * .1f*/);
//			Debug.DrawRay (rigidbody2D.transform.position + rigidbody2D.transform.right*.08f - rigidbody2D.transform.up*.1f, rigidbody2D.transform.up /*+ rigidbody2D.transform.right * .1f*/);
//
//			if (Vector2.Distance(lefthit.point, rigidbody2D.transform.position) < .2
//			    && Vector2.Distance(righthit.point, rigidbody2D.transform.position) < .2) {
//				print ("both");
//				//rigidbody2D.AddTorque(-.4f);
//				//rigidbody2D.velocity = -1 * rigidbody2D.transform.up;
//				//rigidbody2D.angularVelocity = -rigidbody2D.angularVelocity;
//				//rigidbody.angularVelocity -=
//				rigidbody2D.angularVelocity += .01f;
//
//			}
//			else if (Vector2.Distance(lefthit.point, rigidbody2D.transform.position) < .2) {
//				print ("left");
//				//rigidbody2D.AddTorque(-.05f);
//				rigidbody2D.angularVelocity += .01f;
//				//rigidbody2D.velocity = 1 * rigidbody2D.transform.up;
//			}
//			else if (Vector2.Distance(righthit.point, rigidbody2D.transform.position) < .2) {
//				print ("right");
//				//rigidbody2D.AddTorque(.075f);
//				rigidbody2D.angularVelocity -= .01f;
//				//rigidbody2D.velocity = 1 * rigidbody2D.transform.up;
//			}
		}

		tired++;
	}

	private void ApplyTowardsTarget(Vector2 target, ref Vector2 force)
	{
		force += new Vector2 (target.x - Position.x, target.y - Position.y).normalized;
	}
}
