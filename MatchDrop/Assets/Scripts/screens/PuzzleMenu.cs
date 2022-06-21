using UnityEngine;
using System.Collections;

public class PuzzleMenu : MonoBehaviour {
	
	public Vector2 scrollPosition;
	
	public static int buttonH = 50;
	public static int buttonW = buttonH * 4;
	protected int highLevel;
	GUIStyle btnStyle; 
	GUIStyle lockedStyle; 

	public int btnNum= 50;
	
	float scrollVelocity;
	float timeTouchPhaseEnded;
	
	// Use this for initialization
	void Start () {
		int currentLevel = PlayerPrefs.GetInt(PuzzleManager.PREF_PUZZLE_LEVEL, 0);

		scrollPosition = new Vector2(0, 0);//currentLevel * buttonH);

		highLevel = PlayerPrefs.GetInt(PuzzleManager.PREF_PUZZLE_HIGH, 0);

		Debug.Log(highLevel);
		
		btnStyle = new GUIStyle();
		btnStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		btnStyle.normal.background = new Texture2D(20, 35);
		btnStyle.hover.background = new Texture2D(20, 35);
		btnStyle.normal.textColor = new Color(0f, 0f, 0f);
		btnStyle.alignment = TextAnchor.MiddleCenter;
		btnStyle.fontSize = (int)(Screen.height/20);

		lockedStyle = new GUIStyle();
		lockedStyle.font = (Font)Resources.Load("Fonts/BradBunR");
		lockedStyle.normal.background = new Texture2D(20, 35);
		lockedStyle.hover.background = new Texture2D(20, 35);
		lockedStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
		lockedStyle.alignment = TextAnchor.MiddleCenter;
		lockedStyle.fontSize = (int)(Screen.height/25);
	}

	// Update is called once per frame
	void Update () {
		bool touchDevice = false;

		#if UNITY_ANDROID
		touchDevice = true;
		#endif
		
		#if UNITY_IPHONE
		touchDevice = true;
		#endif
		
		#if UNITY_EDITOR
		touchDevice = false;
		#endif
		
		#if UNITY_STANDALONE_OSX
		touchDevice = false;
		#endif
		
		#if UNITY_STANDALONE_WIN
		touchDevice = false;
		#endif

		float inertiaDuration = 1;

		if(touchDevice){
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Moved){
					scrollPosition.y += touch.deltaPosition.y * 4;
					scrollVelocity = touch.deltaPosition.y * 4;
				} else if (touch.phase == TouchPhase.Ended) {
					//timeTouchPhaseEnded = Time.time;
				}
			}
		}

		
		scrollVelocity -= Time.deltaTime;


		//scrollVelocity = 5;
		if ( scrollVelocity > 0.0f )
		{
			scrollPosition.y += scrollVelocity;
		}
	}
	
	void OnGUI () {
		
		int width  = Screen.width;

		scrollPosition = GUI.BeginScrollView(new Rect(width/2 - buttonW/2, buttonH, buttonW * 1.2f, 13 * buttonH), 
		                                     scrollPosition, 
		                                     new Rect(0, 0, buttonW, buttonH * btnNum));
//		scrollPosition = GUI.BeginScrollView(new Rect(10, 10, 100, 300), 
//		                                     scrollPosition, 
//		                                     new Rect(0, 0, 100, 200));


		for(int i = 0; i < btnNum; i++){
			if(highLevel >= i){
				if(GUI.Button(new Rect(0, i * buttonH, buttonW, buttonH), "Level " + i, btnStyle)){
						PlayerPrefs.SetInt(PuzzleManager.PREF_PUZZLE_LEVEL, i);
						Application.LoadLevel("PuzzleScreen");
				}
			} else {
				GUI.Label(new Rect(0, i * buttonH, buttonW, buttonH), "LOCKED", lockedStyle);
			}
		}

//		if(GUI.Button(new Rect(0, 0, buttonW, buttonH * 10), "Top-left")){
//			PlayerPrefs.SetInt(PuzzleManager.PREF_PUZZLE_LEVEL, 0);
//			Application.LoadLevel("PuzzleScreen");
//		}
//		//		GUI.Button(new Rect(120, 0, 100, 20), "Top-right");
//		GUI.Button(new Rect(0, 180, buttonW, buttonH), "Bottom-left");
//		//		GUI.Button(new Rect(120, 180, 100, 20), "Bottom-right");
		GUI.EndScrollView();
	}
}
