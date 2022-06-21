using UnityEngine;
using System.Collections;
using SimpleJSON;

public class PuzzleLevel : Level {

	const string JSON_HINT = "hint";
	const string JSON_FAIL = "fail";
	const string JSON_BROADCAST = "broadcast";
	
	public string hint;
	public string fail;
	public string broadcast;
	
	public PuzzleLevel(JSONNode json): base (json){
				
		hint = json[JSON_HINT];
		fail = json[JSON_FAIL];
		broadcast = json[JSON_BROADCAST];
	}
	
	public bool levelComplete(){
//		if(currentStep >= steps.Length){
//			return true;
//		}
		return false;
	}
	
	public override void Activate(){
	}
	
	public override void Deactivate(){
		
	}
}

