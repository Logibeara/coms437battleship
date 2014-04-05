using UnityEngine;
using System.Collections;

enum CrewMemberStatus
{
	IDLE_WANDER,
	TIRED,
	PERFORM_JOB
}

public class CrewMember : MonoBehaviour {

	private Vector2 position;
	private Vector2 target;
	private Vector2 intermediateTarget;

	// Use this for initialization
	void Start () {
		position = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
