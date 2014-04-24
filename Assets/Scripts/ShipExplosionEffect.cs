using UnityEngine;
using System.Collections;

public class ShipExplosionEffect : MonoBehaviour {
		
		private Transform hitPos1;
		private Transform hitPos2;
		private Transform hitPos3;
		
		public Object partSys;
		
		private GameObject explosion1;
		private GameObject explosion2;
		private GameObject explosion3;
		
		public AudioSource gunExplosionSound;
		
		
		//sound
		
		// Use this for initialization
		void Start () {
			string pname =  this.transform.parent.name;
			hitPos1 = this.transform.FindChild("HitPos1");
			hitPos2 = this.transform.FindChild("HitPos2");
			hitPos3 = this.transform.FindChild("HitPos3");
			
			//partSys = Resources.Load ("particle/Explosion06");
			gunExplosionSound = this.GetComponent<AudioSource> ();
			
		}
		
		
	int hitCounter = 0;
		public void SmallHit()
		{
			
		if (hitCounter == 0) {
			explosion1 = Instantiate (partSys, hitPos1.position, hitPos1.rotation) as GameObject;
			Destroy (explosion1, 10);
		}else if(hitCounter == 1)
		{
			explosion2 = Instantiate (partSys, hitPos2.position, hitPos2.rotation) as GameObject;
			Destroy (explosion2, 10);
		}else if(hitCounter == 2)
		{
			explosion3 = Instantiate (partSys, hitPos3.position, hitPos3.rotation) as GameObject;
			Destroy (explosion3, 10);

		}
		
			hitCounter += Random.Range(0,3);
			hitCounter = hitCounter % 3;
		}
	public void KillExplosion()
	{
			


			explosion1 = Instantiate (partSys, hitPos1.position, hitPos1.rotation) as GameObject;
		//	explosion1.GetComponent<ParticleSystem> ().startSpeed = 1;
			Destroy (explosion1, 10);
			explosion2 = Instantiate (partSys, hitPos2.position, hitPos2.rotation) as GameObject;
		//	explosion2.GetComponent<ParticleSystem> ().startSpeed = 1;
			Destroy (explosion2, 10);
			explosion3 = Instantiate (partSys, hitPos3.position, hitPos3.rotation) as GameObject;explosion1.GetComponent<ParticleSystem> ().startSpeed = 1;
		//	explosion3.GetComponent<ParticleSystem> ().startSpeed = 1;
			Destroy (explosion3, 10);
			
	}

}
