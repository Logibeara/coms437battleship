using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battleship : MonoBehaviour {

	private List<CrewMember> crewMembers;
	private int numCrewMembers = 1;
	private List<Station> stations;
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
			crewMember.Position = crewPos[i%crewPos.Length];
			crewMembers.Add(crewMember);
		}

		stations = new List<Station> ();
		//For debugging, create a crew member
		//crewMembers.Add (Instantiate (Resources.Load ("Prefabs/CrewMember")) as CrewMember);

		//find all game objects htat have stations and add them to a list.
		GameObject[] objectArr = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objectArr) 
		{
			Station script = obj.GetComponent (typeof(Station)) as Station;
			if (script != null)
			{
				stations.Add (script);
			}
		}
		Debug.Log (stations.Count().ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}