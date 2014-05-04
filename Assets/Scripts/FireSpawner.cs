using UnityEngine;
using System.Collections;

public class FireSpawner : MonoBehaviour {

	Transform[] fireLocs;
	// Use this for initialization
	void Start () {
		fireLocs = this.GetComponentsInChildren<Transform> ();
	
	}
	public Vector2 NextRanLoc ()
	{
		int k = Random.Range (0, fireLocs.Length);
		Vector2 loc = new Vector2 ();
		loc.x = fireLocs [k].position.x;
		loc.y = fireLocs [k].position.y;
		return loc;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
