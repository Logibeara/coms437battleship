using UnityEngine;
using System.Collections;

public class GuiLayer : MonoBehaviour {

	private Camera camera1;
	private Camera camera2;
	private Camera camera3;

	private int cameraState = 3;
	Vector3 scene1Position;
	Quaternion scene1Rotation;
	Vector3 scene2Position;
	Quaternion scene2Rotation;
	EnemyBattleship enemyBattleship;

	//Progress bar data
	float progress = 0;
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	bool fightingEnemyShip = true;

	//level parameters
	int totalShips = 3;
	int shipsDestroyed = 0;
	float healthPerShip = 100;

	public string notification = "An Enemy Battleship has arrived!";
	// Use this for initialization
	void Start () {
		Application.LoadLevelAdditive(1);
		
		GameObject camera1GameObject = GameObject.FindGameObjectsWithTag ("MainCamera")[0];

		camera1 = camera1GameObject.GetComponent<Camera>();


		//mainCamera.transform.position = scene1Position;
		//mainCamera.transform.rotation = scene1Rotation;
	}

	void Awake()
	{
		//GameObject gobj = (GameObject.FindWithTag ("EnemyBattleship") as GameObject);
		//enemyBattleship = gobj.GetComponent (typeof(EnemyBattleship)) as EnemyBattleship;
	}

	void OnGUI () {

		GUIStyle style = new GUIStyle ();

		if(GUI.Button(new Rect(0,0,Screen.width/8,Screen.height/8), "<size=" + Screen.width/30 + "> Alt\nView</size>")) {

			if(cameraState == 1)
			{
				camera2.enabled = true;

				camera3.enabled = false;
				camera1.enabled = false;
				cameraState = 2;
			}
			else if (cameraState == 2)
			{
				camera3.enabled = true;
	
				camera1.enabled = false;
				camera2.enabled = false;

				cameraState = 3;
			}
			else //cameraState == 3
			{
				camera1.enabled = true;

				camera2.enabled = false;
				camera3.enabled = false;
				cameraState = 1;
			}
		}

		if (enemyBattleship == null) {
			enemyBattleship = (GameObject.FindGameObjectWithTag ("EnemyBattleship") as GameObject).GetComponent<EnemyBattleship> ();

		}
		//display notifcations and  in bottom left corner

		if (fightingEnemyShip) {

				GUI.Box (
		new Rect (Screen.width * 1f / 8, Screen.height * 15f / 16, Screen.width * 6f / 8, Screen.height / 14), "<size=" + Screen.width / 30 + "> " +
						notification + "\n" +
						"Ship Health: " + enemyBattleship.GetCurrentHealth () +
						"</size>"
				);
		} else {

				GUI.Box (
			new Rect (Screen.width * 1f / 8, Screen.height * 15f / 16, Screen.width * 6f / 8, Screen.height / 14), "<size=" + Screen.width / 30 + "> " +
						notification +
						"</size>"
				);
		}


		//progress bar
		
		Vector2 pos  = new Vector2(Screen.width * (1f-1f/32), 0f);
		Vector2 size = new Vector2(Screen.width * 1f/32, Screen.height);
		GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), progressBarEmpty);
		GUI.DrawTexture(new Rect(pos.x, size.y*(1-Mathf.Clamp01(progress)), size.x , size.y*Mathf.Clamp01(progress)), progressBarFull);

	}
	// Update is called once per frame
	float time = 0.0f;
	void Update () {
		//move camera to other scene view
		if (camera2 == null) 
		{
			GameObject camera2GameObject = GameObject.FindGameObjectsWithTag ("Scene2Camera") [0];
			camera2 = camera2GameObject.GetComponent<Camera> ();
		}
		if(camera3 == null)
		{
			GameObject camera3GameObject = GameObject.FindGameObjectWithTag ("CinematicCamera");
			camera3 = camera3GameObject.GetComponent<Camera>();
			if(camera3 != null)
			{
				camera2.enabled = false;
				camera1.enabled = true;
			}
		}

		//progress = (max health *(ships destroyed) + damage to current ship) / (total ships * max health)

		if (fightingEnemyShip) {
			progress = (float)(healthPerShip * shipsDestroyed + enemyBattleship.startingHealth - enemyBattleship.currentHealth) / (totalShips * healthPerShip);
		} else {
			progress = (float)(healthPerShip * shipsDestroyed ) / (totalShips * healthPerShip);
		}

		if (progress > .999f) 
		{
			Application.LoadLevel(2);
		}
		if (enemyBattleship != null)
		{
			healthPerShip = (float)enemyBattleship.startingHealth;
			if(enemyBattleship.fsm_state != EnemyBattleship.FSM_State.Alive)
			{
				if(time == 0.0f)
				{
					notification = "Enemy Battleship Defeated!";
					fightingEnemyShip = false;
					time = Time.time;
					shipsDestroyed ++;
				}
			}
		}
		if (time != 0.0f) 
		{
			if(time + 5< Time.time && enemyBattleship.fsm_state == EnemyBattleship.FSM_State.Dead)
			{
				//reset
				notification = "An Enemy Battleship has arrived!";
				fightingEnemyShip = true;
				enemyBattleship.ResetShip();
				enemyBattleship.xOffset = nextOffset();
				time = 0.0f;
			}
		}

	}

	float nextOffset()
	{
		float[] offsets = new float[]{-2f,-1.5f,1.5f,2f};
		int k = Random.Range (0, offsets.Length - 1);
		return offsets[k];
	}
}
