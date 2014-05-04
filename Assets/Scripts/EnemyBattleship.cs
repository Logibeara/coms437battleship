using UnityEngine;
using System.Collections;

public class EnemyBattleship : MonoBehaviour {
	private double startingHealth = 500;
	public double currentHealth;
	// Use this for initialization
	ShipExplosionEffect effect;
	Animator animator;
	
	private SplashEffect splashEffect;
	public FSM_State fsm_state;
	Transform[] splashLocations;
	public enum FSM_State
	{
		Alive,
		Sinking,
		Dead
	}
	void Start () {
		//mainGun1 = GameObject.Find("EnemyBattleshipExMainGun1") as GameObject;
		//mainGun2 = GameObject.Find("EnemyBattleshipExMainGun2") as GameObject;
		//mainGun3 = GameObject.Find("EnemyBattleshipExMainGun3") as GameObject;
		currentHealth = startingHealth;
		
		splashEffect = this.GetComponent<SplashEffect> ();

		effect = this.GetComponent<ShipExplosionEffect> ();
		fsm_state = FSM_State.Alive;
		animator = this.GetComponent<Animator> ();

		splashLocations = new Transform[5];
		for (int i = 0; i < 5; i++) 
		{
			splashLocations[i] = this.transform.FindChild("InnerTransform").FindChild("SplashLocation"+i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (fsm_state) 
		{
			case(FSM_State.Alive):
				break;
			case(FSM_State.Sinking):
				animator.SetBool("Sinking",true);
				break;
			case(FSM_State.Dead):
			break;

		}
	}

	public void DoDamage(double amount)
	{
		if (fsm_state == FSM_State.Alive) {
			effect.SmallHit ();
			currentHealth -= amount;
			if (currentHealth <= 0) {
	
					currentHealth = 0;
	
					effect.KillExplosion ();
					fsm_state = FSM_State.Sinking;
			}
		}
	}

	public void Miss()
	{
		splashEffect.MakeSplash ();
	}
	public double GetCurrentHealth()
	{
		return currentHealth;
	}
}
