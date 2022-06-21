using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {
	
	public int firstLevel;
	string screenName = "GAME OVER";
	string btnName = "PLAY AGAIN";
	string scoreStr = "FINAL SCORE: ";
	GUIStyle guiStyle; 
	GUIStyle buttonStyle; 
	Text scoreTxt;
	float pixelsToUnits = 100;
	float halfHeight = Screen.height * 0.5f;
	
	// Use this for initialization
	void Start () {
		buttonStyle = new GUIStyle();
		buttonStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		buttonStyle.normal.background = new Texture2D(120, 35);
		buttonStyle.hover.background = new Texture2D(120, 35);
		buttonStyle.normal.textColor = new Color(0f, 0f, 0f);
		buttonStyle.alignment = TextAnchor.UpperCenter;
		buttonStyle.fontSize = (int)(Screen.width/15);
		
		guiStyle = new GUIStyle();
		guiStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		guiStyle.normal.background = new Texture2D(120, 35);
		guiStyle.hover.background = new Texture2D(120, 35);
		guiStyle.normal.textColor = new Color(0f, 0f, 0f);
		guiStyle.hover.textColor = new Color(0f, 0f, 128f);
		guiStyle.alignment = TextAnchor.UpperCenter;
		guiStyle.fontSize = (int)(Screen.width/20);

		scoreStr = scoreStr + GameManager.score;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI () {
		
		//style.font = Resources.Load<Font>("Font/BradBunR", typeof(Font));
		// Make a background box
		
		int height = Screen.height;
		int width  = Screen.width;
		
		GUI.Box(new Rect(0, 0, width, height), screenName, buttonStyle);

		Vector2 textSize = buttonStyle.CalcSize(new GUIContent(btnName));

		if(GUI.Button(new Rect(
			width/2 - textSize.x/2,
			height * .75f - textSize.y/2,
			textSize.x, textSize.y), btnName, guiStyle)) {
			Application.LoadLevel("StartScreen");
		}
		
		
		if(GUI.Button(new Rect(10, 10, 80, 80), "Main")) {
			Application.LoadLevel(0);
		}

		textSize = buttonStyle.CalcSize(new GUIContent(scoreStr));
		GUI.Label(new Rect(width/2 - textSize.x/2, height/2 - textSize.y/2, textSize.x, textSize.y), scoreStr, buttonStyle);
	}
	
}

