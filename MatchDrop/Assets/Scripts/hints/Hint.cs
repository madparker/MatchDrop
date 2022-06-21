using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Hint {
	
	const string JSON_HINT = "hint";
	const string JSON_TRIGGER = "trigger";
	const string JSON_FUNCTION = "function";
	const string JSON_HIGHLIGHT = "highlight";
	const string JSON_SHOWAGAIN = "showAgain";
	const string JSON_BTNTEXT1 = "buttonText1";
	const string JSON_BTNTEXT2 = "buttonText2";

	public string function;
	public string trigger;
	public string popUpString;
	public string focus;
	public string buttonTxt1;
	public string buttonTxt2;

	public bool isTriggered;
	public bool isDone;

	public bool showAgain = false;

	public Hint(JSONNode node){
		popUpString = node[JSON_HINT].Value;
		trigger = node[JSON_TRIGGER].Value;
		function = node[JSON_FUNCTION].Value;
		focus = node[JSON_HIGHLIGHT].Value;
		buttonTxt1 = node[JSON_BTNTEXT1].Value;
		buttonTxt2 = node[JSON_BTNTEXT2].Value;

		showAgain = node[JSON_SHOWAGAIN].AsBool;

		isTriggered = false;
		isDone = false;
	}
}
