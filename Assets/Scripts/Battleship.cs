using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battleship : MonoBehaviour {

	private List<CrewMember> crewMembers;

	// Use this for initialization
	void Start () {
		crewMembers = new List<CrewMember> ();
		//For debugging, create a crew member
		crewMembers.Add (Instantiate (Resources.Load ("Prefabs/CrewMember")) as CrewMember);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
