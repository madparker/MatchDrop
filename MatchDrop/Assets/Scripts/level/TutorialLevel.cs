using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

public class TutorialLevel : PuzzleLevel {
	
	const string JSON_STEPS = "steps";

	public TutorialStep[] steps;

	private int CurrentStep{
		get{
			return currentStep;
		}

		set{
			steps[currentStep].Deactivate();
			currentStep = value;
			steps[currentStep].Activate();
		}
	}

	private int currentStep;

	public TutorialLevel(JSONNode json): base (json){

		JSONArray jsonSteps = json[JSON_STEPS].AsArray;

		steps = new TutorialStep[jsonSteps.Count];

		NewTutorialManager tm = (GameManager.getGameManager().GetComponent<NewTutorialManager>());

		for(int i = 0; i < steps.Length; i++){

			
			steps[i] = new TutorialStep(tm, jsonSteps[i]);
		}
	}

//	public TutorialLevel(TutorialStep[] steps, string nexts, string[] lines): base(nexts, lines){
//		currentStep = 0;
//		this.steps = steps;
//	}

	public bool step(){
		if(currentStep != steps.Length){
			if(steps[currentStep].stepDelegate()){

				currentStep++;

				if(currentStep == steps.Length){
					return false;
				} else {
					steps[currentStep - 1].Deactivate();
					steps[currentStep].Activate();
				}
			}
		}
		return true;
	}

	public bool GetAllowPrev(){
		if(currentStep >= steps.Length){
			return false;
		}

		return steps[currentStep].allowPrev;
	}
	
	public int GetDropCol(){
		if(currentStep >= steps.Length){
			return -1;
		}
		return steps[currentStep].dropCol;
	}

	public void StepLevel(TutorialManager tm){
		int stepNum = currentStep;

		if(stepNum >= steps.Length){
			stepNum = steps.Length - 1;
		}

		tm.setText(steps[stepNum].text);
	}
	
	public bool levelComplete(){
		if(currentStep >= steps.Length){
			return true;
		}
		return false;
	}

	public TutorialStep GetCurrentStep(){
		return steps[currentStep];
	}

	public override void Activate(){
		CurrentStep = 0;
		steps[CurrentStep].Activate();
	}
	
	public override void Deactivate(){
		
	}
}
