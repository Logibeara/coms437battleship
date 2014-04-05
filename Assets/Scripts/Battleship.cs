using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battleship : MonoBehaviour {

	private List<CrewMember> crewMembers;
	private List<Station> stations;

	// Use this for initialization
	void Start () {
		crewMembers = new List<CrewMember> ();
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
