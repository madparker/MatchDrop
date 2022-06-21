using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	//public GUITexture gt;

	public int firstLevel;
	string screenName = "MATCH DROP";
	string locked = "LOCKED";
	string tutorialBtn = "TUTORIAL";
	string endlessBtn = "ENDLESS";
	string puzzleBtn = "PUZZLE\n100/100";
	string challengeBtn = "CHALLENGE";
	GUIStyle imgStyle; 
	GUIStyle guiStyle; 
	GUIStyle buttonStyle; 
	GUIStyle scoreStyle; 
	GUIStyle lockedStyle; 
	//GUIText scoreTxt;
	float pixelsToUnits = 100;
	float halfHeight = Screen.height * 0.5f;
	string highScoreStr = "HIGH SCORE: ";
	string levelStr = "HIGH LEVEL: ";
	string averageStr = "AVERAGE SCORE: ";

	public static int MODE_INTRO = 0;
	public static int MODE_PUZZLE = 1;
	public static int MODE_ENDLESS = 1;
	public static int MODE_CHALLENGE = 2;

	public static int mode = MODE_INTRO;
	
	// Use this for initialization
	void Start () {

		imgStyle = new GUIStyle();
		imgStyle.font = (Font)Resources.Load("Fonts/BradBunR");
//		imgStyle.normal.background = new Texture2D(120, 35);
//		imgStyle.hover.background = new Texture2D(120, 35);
//		imgStyle.normal.textColor = new Color(0f, 0f, 0f);
//		imgStyle.hover.textColor = new Color(0f, 0f, 128f);
		imgStyle.alignment = TextAnchor.MiddleCenter;
		imgStyle.fontSize = 20;

		buttonStyle = new GUIStyle();
		buttonStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		buttonStyle.normal.background = new Texture2D(120, 35);
		buttonStyle.hover.background = new Texture2D(120, 35);
		buttonStyle.normal.textColor = new Color(0f, 0f, 0f);
		buttonStyle.hover.textColor = new Color(0f, 0f, 128f);
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		buttonStyle.fontSize = (int)(Screen.width/10);
		
		guiStyle = new GUIStyle();
		guiStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		guiStyle.normal.background = new Texture2D(120, 35);
		guiStyle.hover.background = new Texture2D(120, 35);
		guiStyle.normal.textColor = new Color(0f, 0f, 0f);
		guiStyle.alignment = TextAnchor.UpperCenter;
		guiStyle.fontSize = (int)(Screen.width/6);
		
		scoreStyle = new GUIStyle();
		scoreStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		scoreStyle.normal.background = new Texture2D(120, 35);
		scoreStyle.hover.background = new Texture2D(120, 35);
		scoreStyle.normal.textColor = new Color(0f, 0f, 0f);
		scoreStyle.alignment = TextAnchor.MiddleCenter;
		scoreStyle.fontSize = (int)(Screen.width/25);
		
		lockedStyle = new GUIStyle();
		lockedStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		lockedStyle.normal.background = new Texture2D(20, 35);
		lockedStyle.hover.background = new Texture2D(20, 35);
		lockedStyle.normal.textColor = new Color(0f, 0f, 0f);
		lockedStyle.alignment = TextAnchor.MiddleCenter;
		lockedStyle.fontSize = (int)(Screen.width/10);

		
		int highScore = PlayerPrefs.GetInt(GameManager.PREF_HIGH_SCORE);
		int highLevel = PlayerPrefs.GetInt(GameManager.PREF_HIGH_LEVEL);
		int totalGames = PlayerPrefs.GetInt(GameManager.PREF_GAMES_PLAYED);
		int totalScore = PlayerPrefs.GetInt(GameManager.PREF_TOTAL_SCORE);
		
		highScoreStr = highScoreStr + highScore;
		levelStr = levelStr + highLevel;
		if(totalGames > 0){
			averageStr = averageStr + (totalScore/totalGames);
		} else {
			averageStr = averageStr + "0";
		}

		mode = PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL, MODE_INTRO);
		
		GameManager.hasOverlay = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI () {
		
		//style.font = Resources.Load<Font>("Font/BradBunR", typeof(Font));
		// Make a background box
		
		int height = Screen.height;
		int width  = Screen.width;
		
//		GUI.Box(new Rect(0, 0, width, height/8f), screenName, guiStyle);
		
		Vector2 textSize = buttonStyle.CalcSize(new GUIContent(endlessBtn));

//		if(GUI.Button(new Rect(
//			width/2 - textSize.x,
//			height * .25f - textSize.y/2,
//			textSize.x * 2, textSize.y), tutorialBtn, buttonStyle)) {
//			Application.LoadLevel("TutorialScreen");
//		}
//
//		if(GUI.Button(new Rect(
//			width/2 - textSize.x,
//			height * .4f - textSize.y/2,
//			textSize.x * 2, textSize.y), puzzleBtn, buttonStyle)) {
//			
//			if(mode >= MODE_PUZZLE){
//				Application.LoadLevel("PuzzleScreen");
//			}
//		}
//
//		if(mode < MODE_PUZZLE){
//			GUI.Label(new Rect(width/2 - textSize.x, height * .4f - textSize.y/2,
//			                   textSize.x * 2, textSize.y), 
//			          locked, lockedStyle);
//		}
//
//		if(GUI.Button(new Rect(
//			width/2 - textSize.x,
//			height * .5f - textSize.y/2,
//			textSize.x * 2, textSize.y), endlessBtn, buttonStyle)) {
//			if(mode >= MODE_ENDLESS){
//				Application.LoadLevel("MainGame");
//			}
//		}
//		
//		if(mode < MODE_ENDLESS){
//			GUI.Label(new Rect(width/2 - textSize.x, height * .5f - textSize.y/2,
//			                   textSize.x * 2, textSize.y), 
//			          locked, lockedStyle);
//		}
//		
//		if(GUI.Button(new Rect(
//			width/2 - textSize.x,
//			height * .6f - textSize.y/2,
//			textSize.x * 2, textSize.y), challengeBtn, buttonStyle)) {
//			if(mode >= MODE_CHALLENGE){
//				Application.LoadLevel("PuzzleScreen");
//			}
//		}
//		
//		if(mode < MODE_CHALLENGE){
//			GUI.Label(new Rect(width/2 - textSize.x, height * .6f - textSize.y/2,
//			                   textSize.x * 2, textSize.y), 
//			          locked, lockedStyle);
//		}
//
//		
//		if(GUI.Button(new Rect(
//			width/2 - textSize.x,
//			height * .75f - textSize.y/2,
//			textSize.x * 2, textSize.y), "RESET", buttonStyle)) {
//			PlayerPrefs.DeleteAll();
//			Application.LoadLevel("StartScreen");
//		}
		
		float border = width * .05f;
		float midHeight = height * .45f;
		float rectSize = height * .15f;
		float textOffset = rectSize;

//		if(GUI.Button(new Rect(width * .5f - rectSize/2, border + rectSize, rectSize, rectSize), 
//		              Resources.Load<Texture2D>("images/tokens/standard/m2"), imgStyle)){
//			Application.LoadLevel("TutorialScreen");
//		}
//		if(GUI.Button(new Rect(width * .5f - rectSize/2, border + rectSize * 2, rectSize, rectSize), tutorialBtn, imgStyle)){
//			Application.LoadLevel("TutorialScreen");
//		}
//		
//		if(GUI.Button(new Rect(border, midHeight, rectSize, rectSize), 
//		              Resources.Load<Texture2D>("images/tokens/standard/chimp"), imgStyle)){
//			if(mode >= MODE_PUZZLE){
//				Application.LoadLevel("PuzzleScreen");
//			}
//		}
//		if(GUI.Button(new Rect(border,midHeight + rectSize, rectSize, rectSize), puzzleBtn, imgStyle)){
//			if(mode >= MODE_PUZZLE){
//				Application.LoadLevel("PuzzleScreen");
//			}
//		}
//
//		if(mode < MODE_PUZZLE){
//			GUI.Label(new Rect(border,midHeight, rectSize, rectSize), 
//			          locked, lockedStyle);
//		}
//
//		//Arcade
//		if(GUI.Button(new Rect(width - rectSize - border, midHeight, rectSize, rectSize), 
//		              Resources.Load<Texture2D>("images/tokens/standard/lion"), imgStyle)){
//			if(mode >= MODE_ENDLESS){
//				Application.LoadLevel("MainGame");
//			}
//		}
//		if(GUI.Button(new Rect(width - rectSize - border, midHeight + rectSize, rectSize, rectSize), endlessBtn, imgStyle)){
//			if(mode >= MODE_ENDLESS){
//				Application.LoadLevel("MainGame");
//			}
//		}
//		
//		if(mode < MODE_ENDLESS){
//			GUI.Label(new Rect(width - rectSize - border, midHeight, rectSize, rectSize), 
//			          locked, lockedStyle);
//		}
//
//		//Challenge
//		
//		
//		if(GUI.Button(new Rect(width * .5f - rectSize/2, height - border * 4 - rectSize, rectSize, rectSize), 
//		              Resources.Load<Texture2D>("images/tokens/standard/lion"), imgStyle)){
//			if(mode >= MODE_CHALLENGE){
//				Application.LoadLevel("ChallengeScreen");
//			}
//		}
//		if(GUI.Button(new Rect(width * .5f - rectSize/2, height - border * 4, rectSize, rectSize), "CHALLENGE", imgStyle)){
//			if(mode >= MODE_CHALLENGE){
//				Application.LoadLevel("ChallengeScreen");
//			}
//		}
//		
//		if(mode < MODE_CHALLENGE){
//			GUI.Label(new Rect(width * .5f - rectSize/2, height - border * 4 - rectSize, rectSize, rectSize), 
//			          locked, lockedStyle);
//		}

		//RESET
		if(GUI.Button(new Rect(width * .75f - rectSize/2, height - border * 4 - rectSize, rectSize, rectSize), 
		              Resources.Load<Texture2D>("images/tokens/standard/m3"), imgStyle)){
			PlayerPrefs.DeleteAll();
			Application.LoadLevel("StartScreen");
		}
		if(GUI.Button(new Rect(width * .75f - rectSize/2, height - border * 4, rectSize, rectSize), "RESET", imgStyle)){
			PlayerPrefs.DeleteAll();
			Application.LoadLevel("StartScreen");
		}
		
//		textSize = scoreStyle.CalcSize(new GUIContent(highScoreStr));
//
//		GUI.Label(new Rect(width/2 - textSize.x/2, height * .85f - textSize.y/2, textSize.x, textSize.y), highScoreStr, scoreStyle);
//		textSize = scoreStyle.CalcSize(new GUIContent(levelStr));
//		GUI.Label(new Rect(width/2 - textSize.x/2, height * .875f - textSize.y/2, textSize.x, textSize.y), levelStr, scoreStyle);
//		textSize = scoreStyle.CalcSize(new GUIContent(averageStr));
//		GUI.Label(new Rect(width/2 - textSize.x/2, height * .9f - textSize.y/2, textSize.x, textSize.y), averageStr, scoreStyle);
	}
	
}


