using UnityEngine;
using System.Collections;

public class GunExplosionEffect : MonoBehaviour {

	private Transform firePos1;
	private Transform firePos2;
	private Transform firePos3;

	public Object partSys;

	private GameObject explosion1;
	private GameObject explosion2;
	private GameObject explosion3;

	public AudioSource gunExplosionSound;


	//sound

	// Use this for initialization
	void Start () {
		string pname =  this.transform.parent.name;
		firePos1 = this.transform.FindChild("FirePos1");
		firePos2 = this.transform.FindChild("FirePos2");
		firePos3 = this.transform.FindChild("FirePos3");

		//partSys = Resources.Load ("particle/Explosion06");
		gunExplosionSound = this.GetComponent<AudioSource> ();

	}

	public void Burst()
	{
		Quaternion fireDirection = this.transform.rotation;
		explosion1 = Instantiate (partSys, firePos1.position, firePos1.rotation) as GameObject;
		explosion2 = Instantiate (partSys, firePos2.position, firePos2.rotation) as GameObject;
		explosion3 = Instantiate (partSys, firePos3.position, firePos3.rotation) as GameObject;

		Destroy (explosion1, 10);
		Destroy (explosion2, 10);
		Destroy (explosion3, 10);
		gunExplosionSound.PlayOneShot (gunExplosionSound.clip);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
