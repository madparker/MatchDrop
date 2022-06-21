using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class HintHelper : MonoBehaviour {
	
	public GameObject popup1;
	public GameObject popup2;
	public GameObject scoreLevel;
	public GameObject focus;
	public List<Hint> hints;
	public static Hint currentHint;	

	public static bool slowAnim = false;

	public static HintHelper instance;

	// Use this for initialization
	void Start () {
		TextAsset jsonData = Resources.Load("Files/tutorialLevels") as TextAsset;
		string file = jsonData.text;
		JSONNode node = JSON.Parse(file);
		
		JSONArray hintArray = node["hints"].AsArray;

		hints = new List<Hint>();

		for(int i = 0; i < hintArray.Count; i++){
			Hint hint = new Hint(hintArray[i]);
			hints.Add(hint);
		}

		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		bool newHint = false;

		if(!TokenMovement.inAnim && (currentHint == null || currentHint.isDone)){
			foreach(Hint hint in hints){
				if(hint.trigger.Equals("") && !hint.isDone){
					if(!hint.isTriggered){
						currentHint = hint;
						newHint = true;
						Invoke (hint.function, 0);
					}
					break;
				} else if (!hint.isDone && IsHintCondition(hint.trigger)){
					Debug.Log("YUP!!!");
					currentHint = hint;
					newHint = true;
					SendMessage(hint.function);
//					Debug.Log(hint.function);
//					Invoke (hint.function, 0);
					break;
				}
			}
		}

		if(!newHint && currentHint !=null && currentHint.isDone){
			if(currentHint.showAgain){
				currentHint.isDone = false;
			}
			currentHint = null;

			scoreLevel.SetActive(true);
		}
	}

	public bool IsHintCondition(string hintStr){
		if(hintStr.Contains("Match")){
			if(GridHandler.hasMatch(false)){
				if(hintStr.Contains("Match4")){
					return GridToken.currentStreak == 4;
				} else if(hintStr.Contains("Match5")){
					return GridToken.currentStreak >= 5;
				} else {
					return true;
				}
			}
		}
		return false;
	}

	public void GridOverFlowHint(){
		SetPopup();
		SetHighLight();
	}

	public void SetMatchHighLight(){
		Debug.Log("SetMatchHighLight");

		slowAnim = true;
		SetPopup();
		focus.GetComponent<FocusAreaScript>().startPoint = GridToken.startStreak.transform.position;
		focus.GetComponent<FocusAreaScript>().endPoint = GridToken.endStreak.transform.position;

		focus.GetComponent<FocusAreaScript>().prevPoint =
			GameManager.nextTokenHandler.GetComponent<NextTokenHandler>().getNextBlankPos();
		SetHighLight();
	}


	public void SetMatchHighLight(Vector3 prevPoint){

		focus.GetComponent<FocusAreaScript>().prevPoint = prevPoint;
		SetHighLight();
	}

	public void SetHighLight(){
		if(currentHint == null){
			Debug.Log("NULL currentHint");
		}

		focus.GetComponent<FocusAreaScript>().SetHighLightType(currentHint.focus);
	}

	public void SetPopup(){
		GameManager.hasOverlay = true;
		scoreLevel.SetActive(false);

		GameObject popup = popup1;

		if(currentHint.showAgain){
			popup = popup2;
		}

		popup.SetActive(true);
		popup.GetComponentInChildren<Text>().text = currentHint.popUpString;
		
		Text[] texts = popup.GetComponentsInChildren<Text>();
		
		foreach(Text text in texts){
			if(text.gameObject.name.Equals("BtnText1")){
				text.text = currentHint.buttonTxt1;
			}
			if(text.gameObject.name.Equals("BtnText2")){
				text.text = currentHint.buttonTxt2;
			}
		}
	}
	
	public void DismissReturnPopup(){
		slowAnim = false;
		GameManager.hasOverlay = false;
		popup2.SetActive(false);
		currentHint.isDone = true;
		focus.GetComponent<FocusAreaScript>().SetHighLightType("off");
	}
	
	public void DismissPopup(){
		slowAnim = false;
		currentHint.isDone = true;
		currentHint.showAgain = false;
		GameManager.hasOverlay = false;
		popup1.SetActive(false);
		popup2.SetActive(false);
		focus.GetComponent<FocusAreaScript>().SetHighLightType("off");
	}
}
