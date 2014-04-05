using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battleship : MonoBehaviour {

	private List<CrewMember> crewMembers;
	private int numCrewMembers = 10;

	// Use this for initialization
	void Start () {

		Vector3[] crewPos = {
			transform.FindChild("CrewSpawner1").position,
			transform.FindChild("CrewSpawner2").position};
		crewMembers = new List<CrewMember> ();

		for(int i = 0; i < numCrewMembers; i ++)
		{
			CrewMember crewMember = (
				Instantiate (Resources.Load ("Prefabs/CrewMember")) as GameObject).GetComponent(
				typeof(CrewMember)) as CrewMember;
			Debug.Log("before : " + crewMember.Position );
			crewMember.Position = crewPos[i%crewPos.Length];
			Debug.Log("after : " + crewMember.Position );
			crewMembers.Add(crewMember);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
