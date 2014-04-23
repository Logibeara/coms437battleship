using UnityEngine;
using System.Collections;

public class ExBattleship : MonoBehaviour {
	private GameObject exMainGun1;
	private GameObject exMainGun2;
	private GameObject exMainGun3;
	
	
	private GameObject mainGun1;
	private GameObject mainGun2;
	private GameObject mainGun3;
	// Use this for initialization
	void Start () {
		//find all game objects htat have stations and add them to a list.
		exMainGun1 = GameObject.Find("ExBattleshipExMainGun1") as GameObject;
		exMainGun2 = GameObject.Find("ExBattleshipExMainGun2") as GameObject;
		exMainGun3 = GameObject.Find("ExBattleshipExMainGun3") as GameObject;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mainGun1 == null || mainGun2 == null || mainGun3 == null) {
				GameObject[] objarr = GameObject.FindGameObjectsWithTag ("MainGun") as GameObject[];
	
				if (objarr != null && objarr.Length >= 3) {
						mainGun1 = objarr [0];
						mainGun2 = objarr [1];
						mainGun3 = objarr [2];
				}
		} else {
			exMainGun1.transform.rotation = UnityEngine.Quaternion.Euler(0f, -mainGun1.transform.eulerAngles.z, 0f);
			exMainGun2.transform.rotation = UnityEngine.Quaternion.Euler(0f, -mainGun2.transform.eulerAngles.z, 0f);
			exMainGun3.transform.rotation = UnityEngine.Quaternion.Euler(0f, -mainGun3.transform.eulerAngles.z, 0f);

		}
	}
}
