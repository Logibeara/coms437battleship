using UnityEngine;
using System.Collections;

public class SplashEffect : MonoBehaviour {

		
		public Object partSys;
		
		private Transform[] splashLocations;
		
		public AudioSource gunExplosionSound;
		
		private GameObject splash;
		//sound
		
		// Use this for initialization
		void Start () {
			string pname =  this.transform.parent.name;
			splashLocations = new Transform[5];
			for (int i = 0; i < 5; i++) 
			{
				splashLocations[i] = this.transform.FindChild("InnerTransform").FindChild("SplashLocation"+i);
			}
			
			//partSys = Resources.Load ("particle/Splash");
			//gunExplosionSound = this.GetComponent<AudioSource> ();
			
		}
		
		
		int hitCounter = 0;
		public void MakeSplash()
		{
			
			splash = Instantiate (partSys, splashLocations [hitCounter].position, splashLocations [hitCounter].rotation) as GameObject;
			Destroy(splash, 5);
			
			hitCounter += Random.Range(0,5);
			hitCounter = hitCounter % 5;
		}

}
