using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Battleship : MonoBehaviour {

	private List<CrewMember> crewMembers;
	private int numCrewMembers = 10;
	private List<Station> stations;
	private List<Fire> fires;
	private FireSpawner fireSpawner;

	public List<CrewMember> Crew {get { return crewMembers; }}
	public List<Fire> Fires {get { return fires; }}
	public void ProjectileHit(Vector2 location)
	{
		Fire newFire = (Instantiate (Resources.Load ("Prefabs/Fire")) as GameObject)
			.GetComponent (typeof(Fire)) as Fire;
		
		newFire.FireList = fires;
		

		Vector3 pos = new Vector3 ();
		pos.x = location.x;
		pos.y = location.y;
		pos.z = 0;
		newFire.transform.position = pos;
		
		fires.Add (newFire);
	}
	public void ProjectileHit()
	{
		Fire newFire = (Instantiate (Resources.Load ("Prefabs/Fire")) as GameObject)
			.GetComponent (typeof(Fire)) as Fire;

		newFire.FireList = fires;

		Vector3 location = fireSpawner.NextRanLoc ();

		Vector3 pos = new Vector3 ();
		pos.x = location.x;
		pos.y = location.y;
		pos.z = 0;
		newFire.transform.position = pos;

		fires.Add (newFire);
	}

	// Use this for initialization
	void Start () {
		fires = new List<Fire> ();

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
			crewMember.CrewList = crewMembers;
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
		fireSpawner = this.transform.FindChild("FireSpawner").GetComponent<FireSpawner>();
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}