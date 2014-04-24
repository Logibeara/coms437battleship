using UnityEngine;
using System.Collections;

public class PlayerMainGun : MonoBehaviour {

	private GameObject firePos1;
	private GameObject firePos2;
	private GameObject firePos3;

	public Object particleSystem;

	private GameObject explosion1;
	private GameObject explosion2;
	private GameObject explosion3;


	//sound

	// Use this for initialization
	void Start () {
		firePos1 = GameObject.Find ("FirePos1");
		firePos2 = GameObject.Find ("FirePos2");
		firePos3 = GameObject.Find ("FirePos3");

		particleSystem = Resources.Load ("particle/Explosion06");


	}

	void Burst()
	{
		Quaternion fireDirection = this.transform.rotation;
		explosion1 = Instantiate (particleSystem, firePos1.transform.position, firePos1.transform.rotation) as GameObject;
		explosion2 = Instantiate (particleSystem, firePos2.transform.position, firePos2.transform.rotation) as GameObject;
		explosion3 = Instantiate (particleSystem, firePos3.transform.position, firePos3.transform.rotation) as GameObject;

		Destroy (explosion1, 10);
		Destroy (explosion2, 10);
		Destroy (explosion3, 10);

	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
