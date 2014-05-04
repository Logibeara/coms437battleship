using UnityEngine;
using System.Collections;

public class GuiLayer : MonoBehaviour {

	private Camera camera1;
	private Camera camera2;

	int cameraState = 1;
	Vector3 scene1Position;
	Quaternion scene1Rotation;
	Vector3 scene2Position;
	Quaternion scene2Rotation;
	EnemyBattleship enemyBattleship;


	//Progress bar data
	float progress = 0;
	Vector2 pos  = new Vector2(Screen.width * (1f-1f/32), 0f);
	Vector2 size = new Vector2(Screen.width * 1f/32, Screen.height);
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	bool fightingEnemyShip = true;

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
			//move camera to other scene view
			if (camera2 == null) 
			{
				GameObject camera2GameObject = GameObject.FindGameObjectsWithTag ("Scene2Camera") [0];
				camera2 = camera2GameObject.GetComponent<Camera> ();
			}
			if(cameraState == 0)
			{
				camera2.enabled = true;
				camera1.enabled = false;
				cameraState = 1;
			}else{
				
				camera2.enabled = false;
				camera1.enabled = true;
				cameraState = 0;
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
		GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), progressBarEmpty);
		GUI.DrawTexture(new Rect(pos.x, size.y*(1-Mathf.Clamp01(progress)), size.x , size.y*Mathf.Clamp01(progress)), progressBarFull);

	}
	// Update is called once per frame
	float time = 0.0f;
	void Update () {
		progress = Time.time * 0.01f;

		if (enemyBattleship != null)
		{
			if(enemyBattleship.fsm_state != EnemyBattleship.FSM_State.Alive)
			{
				if(time == 0.0f)
				{
					notification = "Enemy Ship has been defeated!";
					fightingEnemyShip = false;
					time = Time.time;
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
