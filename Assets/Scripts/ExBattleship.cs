using UnityEngine;
using System.Collections;

public class ExBattleship : MonoBehaviour {
	private GameObject exMainGun1;
	private GameObject exMainGun2;
	private GameObject exMainGun3;
	
	
	private GameObject mainGun1;
	private GameObject mainGun2;
	private GameObject mainGun3;

	private GunStation gunStation1;
	private GunStation gunStation2;
	private GunStation gunStation3;

	private GunExplosionEffect explosionEffect1;
	private GunExplosionEffect explosionEffect2;
	private GunExplosionEffect explosionEffect3;
	float hitPercentage  = .5f;
	private SplashEffect splashEffect;


	private EnemyBattleship enemyBattleship;
	
	private ShipExplosionEffect effect;
	// Use this for initialization
	void Start () {
		//find all game objects htat have stations and add them to a list.
		exMainGun1 = GameObject.Find("InnerTransformExMainGun1") as GameObject;
		exMainGun2 = GameObject.Find("InnerTransformExMainGun2") as GameObject;
		exMainGun3 = GameObject.Find("InnerTransformExMainGun3") as GameObject;

		explosionEffect1 = exMainGun1.GetComponent<GunExplosionEffect> ();
		explosionEffect2 = exMainGun2.GetComponent<GunExplosionEffect> ();
		explosionEffect3 = exMainGun3.GetComponent<GunExplosionEffect> ();

		splashEffect = this.GetComponent<SplashEffect> ();
		
		enemyBattleship = (GameObject.FindGameObjectWithTag ("EnemyBattleship") as GameObject).GetComponent<EnemyBattleship> ();
		
		effect = this.GetComponent<ShipExplosionEffect> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (mainGun1 == null || mainGun2 == null || mainGun3 == null) {
				GameObject[] objarr = GameObject.FindGameObjectsWithTag ("MainGun") as GameObject[];
	
				if (objarr != null && objarr.Length >= 3) {
						mainGun1 = objarr [0];
						mainGun2 = objarr [1];
						mainGun3 = objarr [2];

					gunStation1 = mainGun1.GetComponent<GunStation>();
					gunStation2 = mainGun2.GetComponent<GunStation>();
					gunStation3 = mainGun3.GetComponent<GunStation>();

				}

				
		} else {


			exMainGun1.transform.rotation = UnityEngine.Quaternion.Euler(0f, -mainGun1.transform.eulerAngles.z, 0f);
			exMainGun2.transform.rotation = UnityEngine.Quaternion.Euler(0f, -mainGun2.transform.eulerAngles.z, 0f);
			exMainGun3.transform.rotation = UnityEngine.Quaternion.Euler(0f, -mainGun3.transform.eulerAngles.z, 0f);

			if(gunStation1.shootSemaphore > 0)
			{
				gunStation1.shootSemaphore --;
				explosionEffect1.Burst();
				if(Random.Range(0.0f,1.0f) <= hitPercentage)
				{
					enemyBattleship.DoDamage(4);
				}
				else
				{
					enemyBattleship.Miss();
				}

			}
			if(gunStation2.shootSemaphore > 0)
			{

				gunStation2.shootSemaphore --;
				explosionEffect2.Burst();
				if(Random.Range(0.0f,1.0f) <= hitPercentage)
				{

					enemyBattleship.DoDamage(4);
				}
				else
				{
					enemyBattleship.Miss();
				}
			}
			if(gunStation3.shootSemaphore > 0)
			{
				gunStation3.shootSemaphore --;
				explosionEffect3.Burst();
				if(Random.Range(0.0f,1.0f) <= hitPercentage)
				{

					enemyBattleship.DoDamage(4);
				}
				else
				{
					enemyBattleship.Miss();
				}
			}
		}
	}

	public void DoDamage()
	{
		effect.SmallHit ();
	}
	public void Miss()
	{
		splashEffect.MakeSplash ();
	}

}
