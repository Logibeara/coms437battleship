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

	// Use this for initialization
	void Start () {
		Application.LoadLevelAdditive(1);
		
		GameObject camera1GameObject = GameObject.FindGameObjectsWithTag ("MainCamera")[0];


		camera1 = camera1GameObject.GetComponent<Camera>();



		//mainCamera.transform.position = scene1Position;
		//mainCamera.transform.rotation = scene1Rotation;
	
	}

	void OnGUI () {


		if(GUI.Button(new Rect(0,0,50,40), "Alt\nView")) {
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
	}
	// Update is called once per frame
	void Update () {
	
	}
}
