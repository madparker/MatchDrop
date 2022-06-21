using UnityEngine;
using System.Collections;

public class Puzzle2ChallengeMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {

		for(int i = 0; i < 9; i++){
			GameObject gameObject = GameObject.Find("ButtonPuzzleLevel" + (i + 1));
		}

		GameObject next = GameObject.Find("NextButton");
		if(next != null){
			next.GetComponent<ChangeScreenSpriteBtn>().screenName = "ChallengeMenuScreen";
		}
		GameObject back = GameObject.Find("BackButton");
		if(back != null){
			back.GetComponent<ChangeScreenSpriteBtn>().screenName = "ChallengeMenuScreen";
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
