using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

public class TutorialStep {
	
	const string JSON_COND = "stepCondition";
	const string JSON_TEXT = "text";
	const string JSON_COL = "allowedCol";
	const string JSON_TRIGS = "objTriggers";
	const string JSON_PREV = "prev";
	const string JSON_HLAREA = "hlArea";

	public string text;

	public delegate bool StepDelegate();

	public StepDelegate stepDelegate;

	public GameObject[] triggers;

	public int dropCol = -1;
	public bool allowPrev;
	public string highLight;

	public TutorialStep(StepDelegate handler, String text){
		stepDelegate = new StepDelegate(handler);

		this.text = text;
	}
	public TutorialStep(NewTutorialManager tm, JSONNode node){

		string colNumStr = node[JSON_COL].ToString();

		dropCol = -1;

		if(!colNumStr.Equals("")){
			dropCol = node[JSON_COL].AsInt;
		}

		highLight = node[JSON_HLAREA];

		allowPrev = node[JSON_PREV].AsBool;

		JSONArray jTriggers = node[JSON_TRIGS].AsArray;

		triggers = new GameObject[jTriggers.Count];

		for(int i = 0; i < triggers.Length; i++){
			triggers[i] = GameObject.Find(jTriggers[i]);
		}

		stepDelegate = new StepDelegate(tm.GetCondition(node[JSON_COND]));
		this.text = node[JSON_TEXT];
	}

	public void Activate(){
		foreach(GameObject trigger in triggers){
			trigger.GetComponent<SpriteRenderer>().enabled = true;
			trigger.GetComponent<Animation>().enabled = true;
		}

		GameObject.Find("Focus Area").GetComponent<FocusAreaScript>().SetHighLightType(highLight);
	}

	public void Deactivate(){
		foreach(GameObject trigger in triggers){
			trigger.GetComponent<SpriteRenderer>().enabled = false;
			trigger.GetComponent<Animation>().enabled = false;
		}
	}
}
