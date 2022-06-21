using UnityEngine;
using System.Collections;

public class BackToStartGUI  : MonoBehaviour {
	
	public int firstLevel;
	string endlessBtn = "MAIN";
	GUIStyle buttonStyle; 
	float border;
	
	// Use this for initialization
	void Start () {
		buttonStyle = new GUIStyle();
		buttonStyle.font = (Font)Resources.Load("Fonts/Zepto");
		buttonStyle.normal.background = new Texture2D(120, 35);
		buttonStyle.hover.background = new Texture2D(120, 35);
		buttonStyle.normal.textColor = new Color(0f, 0f, 0f);
		buttonStyle.hover.textColor = new Color(0f, 0f, 128f);
		buttonStyle.alignment = TextAnchor.UpperCenter;
		buttonStyle.fontSize = (int)(Screen.width/15);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI () {
		
		int height = Screen.height;
		int width  = Screen.width;

		border = width/20f;
		
		Vector2 textSize = buttonStyle.CalcSize(new GUIContent(endlessBtn));
		
		if(GUI.Button(new Rect(
			width - textSize.x - border,
			border,
			textSize.x * 1.1f, textSize.y), endlessBtn, buttonStyle)) {
			Application.LoadLevel("StartScreen");
		}
	}
	
}


