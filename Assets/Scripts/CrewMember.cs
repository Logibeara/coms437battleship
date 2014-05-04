using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public enum CrewMemberStatus
{
	IDLE_WANDER,
	TIRED,
	PERFORM_JOB,
	INCAPACITATED
}

public class CrewMember : MonoBehaviour {

	#region Private Constants
	private const float VELOCITY_MAX         = 1f;
	private const float WANDER_CIRCLE_RADIUS = 1.5f;
	#endregion

	#region Public Constants
	public float healthMax = 2;
	#endregion

	#region Private Variables

	private int              tiredness          = 0;     //

	//Health
	private float            healthCurrent      = 0;     //
	private int              accumulatedDamage  = 0;     //
	private bool             beingDamaged       = false; // Mutex to ensure that if surrounded by many fires,
	                                                        // only one fire can hurt crewmember
	//
	private List<CrewMember> masterCrewList;             //

	//Job / goal
	private Station          currentJob;                 //
	private Station          lastKnownJob;               //
	private CrewMemberStatus status;                     //

	//Pathing
	private Vector2          targetPosition;             // The position of the current major goal
	private float            wanderCurrentAngle = 0;     //
	private bool             pathRequested = false;      //
	#endregion

	private Barracks barracks;

	//The calculated path
	public Path path;

	
	//The AI's speed per second
	public float speed = 100;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = .1f;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	Seeker seeker;

	SpriteRenderer jobSpriteRenderer;

	#region Public Accessors

	public CrewMemberStatus Status
	{
		get { return status; }
	}

	public float Health
	{
		get { return healthCurrent;}
	}

	public Vector3 Position
	{
		get { return transform.position; }
		set { transform.position = value; }
	}

	public List<CrewMember> CrewList
	{
		get { return masterCrewList; }
		set { masterCrewList = value; }
	}

	public int Tiredness
	{
		get { return tiredness; }
		set { tiredness = value; }
	}

	public Station LastKnownJob
	{
		get { return lastKnownJob; }
		set { lastKnownJob = value; }
	}

	#endregion



	// Use this for initialization
	void Start () {

		//Local variable init
		barracks = FindObjectOfType(typeof(Barracks)) as Barracks;

		jobSpriteRenderer = gameObject.GetComponentInChildren(typeof(SpriteRenderer)) as SpriteRenderer;
		SetJobIcon (null);

		status = CrewMemberStatus.IDLE_WANDER;
		wanderCurrentAngle = Random.value * 360;
		tiredness = 0;
		//barracks = oldbarracksthing.GetComponent (typeof(Station)) as Station;
		//target = barracksScript.getTarget (this);

		seeker = GetComponent<Seeker> ();
		//seeker.StartPath (transform.position, new Vector3(target.x, target.y,0), OnPathComplete);
		healthCurrent = healthMax;

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
			if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
				currentWaypoint++;
				return;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//do damage and see if they are dead
		healthCurrent -= (float)accumulatedDamage * Time.deltaTime;
		if(healthCurrent <= 0)
		{
			if(status != CrewMemberStatus.INCAPACITATED)
			{
				incap();
			}
			healthCurrent = 0;
		}
		if(healthCurrent > healthMax)
		{
			if(status == CrewMemberStatus.INCAPACITATED)
			{
				revive();
			}
			healthCurrent = healthMax;
		}
		accumulatedDamage = 0;
		beingDamaged = false;
		Vector2 targetPos; //temp variable
		Vector2 aggregateForce = Vector2.zero;

		switch(status)
		{
		case CrewMemberStatus.INCAPACITATED:

			break;

		case CrewMemberStatus.IDLE_WANDER:
			if(rigidbody2D.velocity.magnitude <= VELOCITY_MAX)
			{

				//Generate wander target to follow
				Vector2 forwardVec = 4 * (new Vector2(transform.forward.x, transform.forward.y));
				Vector2 circlePos = new Vector2(Position.x,  Position.y) + forwardVec;
				wanderCurrentAngle += (Random.value * 30) - 15;
				targetPos = circlePos + WANDER_CIRCLE_RADIUS * new Vector2(Mathf.Cos(wanderCurrentAngle * Mathf.Deg2Rad), Mathf.Sin(wanderCurrentAngle * Mathf.Deg2Rad));

				ApplyTowardsTarget(targetPos, ref aggregateForce);
			}
			if(Random.value * 1000000 + 1000 <= tiredness)
			{
				SetJobIcon("tired");
				lastKnownJob = currentJob;
				currentJob = barracks;
				status = CrewMemberStatus.PERFORM_JOB;
			}

			break;
//
		case CrewMemberStatus.PERFORM_JOB:
			if(currentJob == null)
			{
				status = CrewMemberStatus.IDLE_WANDER;
			}
			else if(Vector2.Distance(rigidbody2D.transform.position, currentJob.getTarget(this)) < .5f)
			{
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0;
				currentJob.doWork(this);
			}
			else
			{
				//ApplyTowardsTarget(activeJob.getTarget(this), ref aggregateForce);
				Vector2 t =  currentJob.getTarget(this);
				if(path == null && pathRequested == false)
				{
					seeker.StartPath (transform.position, new Vector3(t.x, t.y, 0), OnPathComplete);
					pathRequested = true;
				}
				rigidbody2D.angularVelocity = 0;
			}
			if(Random.value * 100000 + 1000 <= tiredness)
			{
				SetJobIcon("tired");
				lastKnownJob = currentJob;
				currentJob = barracks;
				status = CrewMemberStatus.PERFORM_JOB;
			}
			break;

		//DEPRECATED CASE
		//TODO extract animation logic
		case CrewMemberStatus.TIRED:
			SetJobIcon("tired");
			targetPos = barracks.getTarget(this);

			//If we're at the barracks
			if(Vector2.Distance(rigidbody2D.transform.position, targetPos) < .4f)
			{
				//Stop animation
				(gameObject.GetComponent(typeof(Animator)) as Animator).enabled = false;

				tiredness -= (int)(Random.value * 10);
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.angularVelocity = 0;
			}
			else
			{
				//Go to the barracks
				Vector2 t =  barracks.getTarget(this);
				if(path == null && pathRequested == false)
				{
					seeker.StartPath (transform.position, new Vector3(t.x, t.y, 0), OnPathComplete);
					pathRequested = true;
				}
			}
			if(tiredness <= 0)
			{
				status = CrewMemberStatus.PERFORM_JOB;
				(gameObject.GetComponent(typeof(Animator)) as Animator).enabled = true;
			}
			break;
		}

		if(rigidbody2D.velocity.magnitude > VELOCITY_MAX)
		{
			rigidbody2D.velocity = rigidbody2D.velocity.normalized *  VELOCITY_MAX;
		}
		if(rigidbody2D.angularVelocity > .01f)
		{
			rigidbody2D.angularVelocity = .01f;
		}
		if(rigidbody2D.angularVelocity < -.01f)
		{
			rigidbody2D.angularVelocity = -.01f;
		}

//		//Apply linear force and torque
		rigidbody2D.AddForce(aggregateForce);
		if(aggregateForce.magnitude > .01)
		{
			Vector3 cross = Vector3.Cross(new Vector3(rigidbody2D.velocity.normalized.x, rigidbody2D.velocity.normalized.y, 0), rigidbody2D.transform.up.normalized);
			rigidbody2D.AddTorque((cross.z < 0) ? .005f : -.005f);
		}
//
		//tiredness++;

		PathUpdate();
	}

	public void ResumeLastKnownJob()
	{
		if(lastKnownJob != null)
		{
			SetStation(lastKnownJob);
		}
		else
		{
			status = CrewMemberStatus.IDLE_WANDER;
			SetJobIcon(null);
		}
	}

	public void doDamage()
	{
		if(!beingDamaged &&(currentJob == null || !currentJob.GetType().Equals(typeof(FireStation))))
		{
			beingDamaged = true;
			accumulatedDamage++;
		}
	}

	public void heal()
	{
		accumulatedDamage -= 2;
	}

	public void nullifyJob()
	{
		status = CrewMemberStatus.IDLE_WANDER;
		currentJob = null;
		path = null;
	}

	private void ApplyTowardsTarget(Vector2 target, ref Vector2 force)
	{
		force += new Vector2 (target.x - Position.x, target.y - Position.y).normalized;
	}

	void incap()
	{
		//TODO change animation state to incapped
		(gameObject.GetComponent(typeof(Animator)) as Animator).enabled = false;

		status = CrewMemberStatus.INCAPACITATED;
		healthCurrent = 0;
	}

	void revive()
	{
		//TODO chanve animation state to walking
		(gameObject.GetComponent(typeof(Animator)) as Animator).enabled = true;

		SetJobIcon("tired");
		lastKnownJob = currentJob;
		currentJob = barracks;
		status = CrewMemberStatus.PERFORM_JOB;
	}

	//Gives this crewmember a job
	public void SetStation(Station station)
	{
		nullifyJob ();
		currentJob = station;
		status = CrewMemberStatus.PERFORM_JOB;
		
		if(station is FireStation)
		{
			SetJobIcon ("firefighter");
		}
		else if(station is GunStation)
		{
			SetJobIcon("gunner");
		}
		else if (station is Barracks)
		{
			SetJobIcon("tired");
		}
		//			else if(station is MedicStation)
		//			{
		//				SetJobIcon("medic");
		//			}
		
		targetPosition = station.getTarget (this);
		
		path = null;
		seeker.StartPath (transform.position, new Vector3(targetPosition.x, targetPosition.y, 0), OnPathComplete);
		pathRequested = true;
	}
}
