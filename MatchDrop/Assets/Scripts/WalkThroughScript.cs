using UnityEngine;
using System.Collections;
using SimpleJSON;

public class WalkThroughScript : TutorialManager {

	float lerpMod = 0.25f;
	float lerpPercent = 0;
	GameObject finger;

	public override void Setup ()
	{
		
		scoreTxt.transform.localScale = new Vector3(0, 0, -5);
		
		if(!hasOverlay)
			CurrentLevel = 0;
		
		tLevels = makeLevels();
		levels = tLevels;
		
		SetUpGridAndNext();
		
		GameManager.hasOverlay = false;
		
		Destroy (Next);

		finger = GameObject.Find("Finger");
		
		tLevels[CurrentLevel].Activate();
	}

	public TutorialLevel[] makeLevels(){

		string file = Util.getFileContents("assets/Files/tutorial.json");
		
		JSONNode node = JSON.Parse(file);

		JSONArray jsonLevels = node["levels"].AsArray;
		
		TutorialLevel[] tLevels = new TutorialLevel[jsonLevels.Count];

		for(int i = 0; i < jsonLevels.Count; i++){
			tLevels[i] = new TutorialLevel(jsonLevels[i]);
		}

		return tLevels;
	}

	public override void InputDropToken(){
		
		unlockPrev = tLevels[CurrentLevel].GetAllowPrev();

		int allowedCol = tLevels[CurrentLevel].GetDropCol();
		
		if(allowedCol != -1){
			float fingerLocation = GridHandler.cols[allowedCol];
			lerpPercent += Time.deltaTime * lerpMod;

			if(lerpPercent > 1){
				lerpPercent = 1;
			}

			finger.SetActive(true);
			Vector3 location = Util.ReplaceVector3X(finger.transform.localPosition, fingerLocation);
			finger.transform.position = Vector3.Lerp(finger.transform.position, location, lerpPercent);
		} else {
			finger.SetActive(false);
		}


		if(allowedCol >= 0){
			if (dropCol == allowedCol){
				base.InputDropToken();

				lerpPercent = 0;
			}
		} else {
			base.InputDropToken();
			lerpPercent = 0;
		}
	}
	
}
