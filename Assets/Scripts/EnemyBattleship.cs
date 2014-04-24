using UnityEngine;
using System.Collections;

public class EnemyBattleship : MonoBehaviour {
	private double startingHealth = 500;
	public double currentHealth;
	// Use this for initialization


	void Start () {
		//mainGun1 = GameObject.Find("EnemyBattleshipExMainGun1") as GameObject;
		//mainGun2 = GameObject.Find("EnemyBattleshipExMainGun2") as GameObject;
		//mainGun3 = GameObject.Find("EnemyBattleshipExMainGun3") as GameObject;
		currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoDamage(double amount)
	{
		currentHealth -= amount;
	}

	public double GetCurrentHealth()
	{
		return currentHealth;
	}
}
