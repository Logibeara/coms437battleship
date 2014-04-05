using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battleship : MonoBehaviour {

	private List<CrewMember> crewMembers;
<<<<<<< HEAD
	private int numCrewMembers = 10;
=======
	private List<Station> stations;
>>>>>>> fdbca79d8bcb0ff0afd9dec433211bcd88e17008

	// Use this for initialization
	void Start () {

		Vector3[] crewPos = {
			transform.FindChild("CrewSpawner1").position,
			transform.FindChild("CrewSpawner2").position};
		crewMembers = new List<CrewMember> ();
<<<<<<< HEAD

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

=======
		stations = new List<Station> ();
		//For debugging, create a crew member
		crewMembers.Add (Instantiate (Resources.Load ("Prefabs/CrewMember")) as CrewMember);

		//find all game objects htat have stations and add them to a list.
		GameObject[] objectArr = GameObject.FindObjectsOfType(typeof(MonoBehaviour)) as GameObject[];
		if (objectArr != null) 
		{
			foreach (GameObject obj in objectArr) 
			{
				Station script = obj.GetComponent (typeof(Station)) as Station;
				if (script != null) 
				{
					stations.Add (script);
				}
			}
		}
		//stations = GameObject.FindObjectsOfType (Station) as Station[];
		Debug.Log (stations.ToString());
>>>>>>> fdbca79d8bcb0ff0afd9dec433211bcd88e17008
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
