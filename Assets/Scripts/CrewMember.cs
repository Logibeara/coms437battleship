using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

enum CrewMemberStatus
{
	IDLE_WANDER,
	TIRED,
	PERFORM_JOB
}

public class CrewMember : MonoBehaviour {

	private float wanderCircleRadius = 1f;
	private float wanderAngle;
	private float velocityMax = 1f;
	private int tired;
	public float maxHealth = 10;
	public float health;

	private int dammage = 0;
	private bool beingDammaged = false;

	List<CrewMember> crewList;

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



	//The calculated path
	public Path path;
	public bool pathRequested = false;
	
	//The AI's speed per second
	public float speed = 100;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = .01f;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	Seeker seeker;

	SpriteRenderer jobSpriteRenderer;

	public List<CrewMember> CrewList
	{
		get { return crewList; }
		set { crewList = value; }
	}
	
	/// <summary>
	/// give this crew member a job
	/// </summary>
	/// <param name="station">Station.</param>
	public void SetStation(Station station)
	{
		nullifyJob ();
		if (activeJob != station) {
			activeJob = station;
			status = CrewMemberStatus.PERFORM_JOB;

			if(station is FireStation)
			{
				SetJobIcon ("firefighter");
			}
			else if(station is GunStation)
			{
				SetJobIcon("gunner");
			}
//			else if(station is MedicStation)
//			{
//				SetJobIcon("medic");
//			}
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
		jobSpriteRenderer = gameObject.GetComponentInChildren(typeof(SpriteRenderer)) as SpriteRenderer;
		SetJobIcon ("medic");

		status = CrewMemberStatus.IDLE_WANDER;
		wanderAngle = Random.value * 360;
		tired = 0;
		barracksScript = barracks.GetComponent (typeof(Station)) as Station;

		//target = barracksScript.getTarget (this);

		seeker = GetComponent<Seeker> ();
		//seeker.StartPath (transform.position, new Vector3(target.x, target.y,0), OnPathComplete);
		health = maxHealth;

	}

	private void SetJobIcon(string fileName)
	{
		if(fileName == null)
		{
			jobSpriteRenderer.sprite = null;
			return;
		}
		jobSpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/JobIcon/" + fileName);
	}

	public void OnPathComplete(Path p)
	{
		//Debug.Log ("Yey, we got a path back. Did it have an error? " + p.error);
		if (!p.error) {
				path = p;
				//Reset the waypoint counter
				currentWaypoint = 0;
		}
		pathRequested = false;
	}

	public void PathUpdate()
	{
		if(!Input.GetMouseButton(0))
		{
			if (path == null) {
				//We have no path to move after yet
				return;
			}
			
			if (currentWaypoint >= path.vectorPath.Count) {
				Debug.Log ("End Of Path Reached");
				rigidbody2D.velocity = Vector2.zero;
				path = null;
				return;
			}
			
			//Direction to the next waypoint
			Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			dir *= speed * Time.deltaTime;
			rigidbody2D.velocity =  new Vector2(dir.x, dir.y);
			
			//Check if we are close enough to the next waypoint
			//If we are, proceed to follow the next waypoint
			if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < .1f) {
				currentWaypoint++;
				return;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//do dammage and see if they are dead
		health -= (float)dammage * Time.deltaTime;
		if(health <= 0)
		{
			die();
		}
		if(health > maxHealth)
		{
			health = maxHealth;
		}
		dammage = 0;
		beingDammaged = false;
//		if(Input.GetMouseButtonDown(0))
//		{
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			
//			Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
//			
//			float distance;
//			
//			print ("fire");
//			if (xyPlane.Raycast (ray, out distance)) {
//				print ("hit");
//				Vector3 hitPoint = ray.GetPoint(distance);
//				target = new Vector2(hitPoint.x, hitPoint.y);
//				
//				seeker.StartPath (transform.position, new Vector3(target.x, target.y,0), OnPathComplete);
//				pathRequested = true;
//			}
//		}
				Vector2 targetPos; //temp variable
		//
				//float aggregateTorque = 0;
				Vector2 aggregateForce = Vector2.zero;
//
//		//Update the intermediate target
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
//
		case CrewMemberStatus.PERFORM_JOB:
			if(activeJob == null)
			{
				status = CrewMemberStatus.IDLE_WANDER;
			}
			else if(Vector2.Distance(rigidbody2D.transform.position, activeJob.getTarget(this)) < .5f)
			{
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0;
				activeJob.doWork(this.Position);
			}
			else
			{
				//ApplyTowardsTarget(activeJob.getTarget(this), ref aggregateForce);
				Vector2 t =  activeJob.getTarget(this);
				if(path == null && pathRequested == false)
				{
					seeker.StartPath (transform.position, new Vector3(t.x, t.y, 0), OnPathComplete);
					pathRequested = true;
				}
				rigidbody2D.angularVelocity = 0;
			}
			if(Random.value * 100000 + 1000 <= tired)
			{
				status = CrewMemberStatus.TIRED;
				//tired = 0;
			}
			break;

		case CrewMemberStatus.TIRED:
			SetJobIcon("tired");
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
				Vector2 t =  barracksScript.getTarget(this);
				if(path == null && pathRequested == false)
				{
					seeker.StartPath (transform.position, new Vector3(t.x, t.y, 0), OnPathComplete);
					pathRequested = true;
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
			rigidbody2D.angularVelocity = .01f;
		}
		if(rigidbody2D.angularVelocity < -.01f)
		{
			rigidbody2D.angularVelocity = -.01f;
		}

//		//After updating the force, perform wall avoidance
//		//Physics2D.Raycast(
//		Debug.DrawRay (rigidbody2D.transform.position + rigidbody2D.transform.up, aggregateForce.normalized);
//
//
//		//Apply linear force and torque
		rigidbody2D.AddForce(aggregateForce);
		if(aggregateForce.magnitude > .01)
		{
			Vector3 cross = Vector3.Cross(new Vector3(rigidbody2D.velocity.normalized.x, rigidbody2D.velocity.normalized.y, 0), rigidbody2D.transform.up.normalized);
			rigidbody2D.AddTorque((cross.z < 0) ? .005f : -.005f);
		}
//
		tired++;

		PathUpdate();
	}

	public void doDammage()
	{
		if(!beingDammaged &&(activeJob == null || !activeJob.GetType().Equals(typeof(FireStation))))
		{
			beingDammaged = true;
			dammage++;
		}
	}

	public void heal()
	{
		dammage -= 2;
	}

	public void nullifyJob()
	{
		status = CrewMemberStatus.IDLE_WANDER;
		activeJob = null;
		path = null;
	}

	private void ApplyTowardsTarget(Vector2 target, ref Vector2 force)
	{
		force += new Vector2 (target.x - Position.x, target.y - Position.y).normalized;
	}

	void die()
	{
		if(crewList != null)
		{
			crewList.Remove (this);
			Destroy(this.gameObject);
		}
		else
		{
			throw new System.ArgumentNullException("crewList is null!!!!");
		}
	}
}
