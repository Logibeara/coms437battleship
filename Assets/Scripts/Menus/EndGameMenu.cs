using UnityEngine;
using System.Collections;

public class EndGameMenu : MonoBehaviour {

	public string TitleMessage = "Not Set";
	private GUIStyle currentStyle = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		InitStyles();
		GUI.Box (
			new Rect (0, 0, Screen.width , Screen.height), "<size=" + Screen.width / 10 + "> " +
			TitleMessage +
			"</size>",
			currentStyle 
			);
	}

	private void InitStyles()
	{
		if( currentStyle == null )
		{
			currentStyle = new GUIStyle( GUI.skin.box );
			currentStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 0f, 0f, 1.0f ) );
		}
	}
	
	
	
	private Texture2D MakeTex( int width, int height, Color col )
		
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}
}
