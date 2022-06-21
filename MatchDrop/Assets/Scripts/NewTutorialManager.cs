using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Reflection;
using System;

public class NewTutorialManager : PuzzleManager
{
	
	protected GameObject textobj;

	public GameObject finger;

	public override void Setup ()
	{
		Destroy(scoreTxt);
		
		makeLevels();
		
		CurrentLevel = 0;
		
		SetUpGridAndNext();
		
		GameManager.hasOverlay = false;
		
		Destroy (Next);
	}
	
	public static void makeLevels(){
		
		if(levels.Length == 0){
			TextAsset jsonData = Resources.Load("Files/tutorialLevels") as TextAsset;
			
			string file = jsonData.text;
			
			JSONNode node = JSON.Parse(file);
			
			JSONArray jsonLevels = node["levels"].AsArray;
			
			TutorialLevel[] pLevels = new TutorialLevel[jsonLevels.Count];
			
			for(int i = 0; i < jsonLevels.Count; i++){
				pLevels[i] = new TutorialLevel(jsonLevels[i]);
			}
			
			levels =  pLevels;
		}
	}
	
	public override void getNextToken(){		
		if(nextTokenHandler.GetComponent<NextTokenHandler>().getCount() > 0){
			base.getNextToken();
		} else {
			if(!inAnim && !GridHandler.inAnim()  && !GridHandler.hasMatch(false)){
				if(gridHandler.GetComponent<GridHandler>().isEmpty()){
					//					if(!successPanel.activeSelf){
					//						currentLevel++;
					//					}
					if(!updateLevel(false)){
						GoToEndScreen();
					}
				} else {
					
//					failPanel.SetActive(true);
//					Text failText = failPanel.GetComponentInChildren<Text>();
//					failText.text = ((PuzzleLevel)levels[CurrentLevel]).fail;
				}
			}
		}
	}
	
	public bool updateLevelOnButton(){
		
		if(CurrentLevel >= levels.Length){
			//			if(PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL) < StartScreen.MODE_PUZZLE){ 
			//				PlayerPrefs.SetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_PUZZLE);
			//			}
			Application.LoadLevel("StartScreen");
			return true;
		}
		
		bool result = base.updateLevel (false);
		
//		hintText.text = ((PuzzleLevel)levels[CurrentLevel]).hint;
		
		return result;
	}

	public override void InputDropToken(){

		TutorialStep currentStep = ((TutorialLevel)levels[CurrentLevel]).GetCurrentStep();

		if(currentStep.dropCol == dropCol){
			base.InputDropToken();
		}
	}

	public TutorialStep.StepDelegate GetCondition(string condStr){
		
		MethodInfo method;
		
		if(condStr.Equals("empty")){
			method = GetType().GetMethod("Empty", 
			                             BindingFlags.Public 
			                             | BindingFlags.Static 
			                             | BindingFlags.FlattenHierarchy);
		} else {
			method = GetType().GetMethod("Drop", 
			                             BindingFlags.Public 
			                             | BindingFlags.Static 
			                             | BindingFlags.FlattenHierarchy);
		}
		
		return (TutorialStep.StepDelegate) Delegate.CreateDelegate
			(typeof(TutorialStep.StepDelegate), method);
	}

	public static bool Drop()
	{
		return false;
	}
	
	public static bool Empty()
	{
		return gridHandler.GetComponent<GridHandler>().isEmpty();
	}

	public override bool updateLevel(bool init){
		
		if(init){
			//			CurrentLevel = 7;
			
			Debug.Log("CurrentLevel:" + CurrentLevel);
			
//			hintText.text = ((PuzzleLevel)levels[CurrentLevel]).hint;
			return base.updateLevel(init);
		} else if(((PuzzleLevel)levels[CurrentLevel]).broadcast != null){
			BroadcastMessage(((PuzzleLevel)levels[CurrentLevel]).broadcast, true);
			return true;
		} else {
			inAnim = true;
			return true;
		}
	}
	
	public void nextPuzzle(){
		CurrentLevel++;
		retryPuzzle();
	}
	
	public void retryPuzzle(){
		Destroy (Next);
		GameManager.hasOverlay = false;
		updateLevelOnButton();
	}
}