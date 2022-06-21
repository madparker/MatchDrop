using UnityEngine;
using System.Collections;

public class StartScreenGui : MonoBehaviour {
		
	public string puzzleScene;
	public string endlessScene;
	public string challengeScene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PuzzleScene(){
		Application.LoadLevel(puzzleScene);
	}
	
	public void EndlessScene(){
		Application.LoadLevel(endlessScene);
	}

	public void ChallengeScene(){
		Application.LoadLevel(challengeScene);
	}
}
