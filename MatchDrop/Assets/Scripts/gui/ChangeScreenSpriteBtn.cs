using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeScreenSpriteBtn : MonoBehaviour {
	
	public string screenName;
	public int clearanceLevel;
	int currentLockLevel;
	GameObject lockLbl;

	public static GameObject[] buttonGroup;

	public bool engage;

	// Use this for initialization
	void Start () {
		currentLockLevel = PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_INTRO);
		
		foreach (Transform child in transform) {
			if (child.gameObject.name == "Lock"){
				lockLbl = child.gameObject;
			}
		}

		buttonGroup = GameObject.FindGameObjectsWithTag("GuiAnim");
//
//			if(buttonGroup == null || buttonGroup.Length > 0){
//				bGroup = buttonGroup;
//
//				Debug.Log (buttonGroup.Length);
//
//				if(bGroup.Length > 0){
//					initBGroup = true;
//					Debug.Log("STOPER");
//
//					Debug.Log (bGroup[0]);
//				}
//			}
//		}

		engage = false;
	}
	
	// Update is called once per frame
	void Update()
	{
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

		Vector2 pos = new Vector2(-100, - 100);

		if(currentLockLevel >= clearanceLevel){
			if(touchDevice && (Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended)){
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				pos = new Vector2(wp.x, wp.y);
			} else if (Input.GetMouseButtonDown(0)){
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos = new Vector2(wp.x, wp.y);
			}

			if (GetComponent<Collider2D>().OverlapPoint(pos))
			{
				engage = true;

				foreach(GameObject b in buttonGroup){
					if(b != null){
						b.AddComponent<Shrinker>();
					}
				}
			}
			if(lockLbl != null){
				lockLbl.SetActive(false);
			}
		} else {
			if(lockLbl != null){
				lockLbl.SetActive(true);
			}
		}

		if(engage && transform.localScale.x == 0){
			Application.LoadLevel(screenName);
		}
	}
}
