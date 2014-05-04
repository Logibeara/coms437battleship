using UnityEngine;
using System.Collections;

public class EnemyBattleship : MonoBehaviour {
	private double startingHealth = 100;
	public double currentHealth;
	// Use this for initialization
	ShipExplosionEffect effect;
	Animator animator;
	
	private SplashEffect splashEffect;
	public FSM_State fsm_state;
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

	}
	
	// Update is called once per frame
	void Update () {

		switch (fsm_state) 
		{
			case(FSM_State.Alive):
				break;
			case(FSM_State.Sinking):
				animator.SetBool("Sinking",true);
				AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
				if (info.IsTag("dead"))
				{
					fsm_state = FSM_State.Dead;
				}
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
	
	public void ResetShip()
	{
		fsm_state = FSM_State.Alive;
		currentHealth = startingHealth;
		animator.SetTrigger ("Reset");
		
		animator.SetBool("Sinking",false);

	}
}
