using UnityEngine;
using System.Collections;

public class PuzzleMenuBtn : MonoBehaviour {

	public int advanceNum = 9;

	void Start () {
		
		if(PuzzleLevelButton.puzzleLevelOffset == 0 && advanceNum < 0){
			Destroy(gameObject);
		}

		if(NewPuzzleManager.levels.Length <= PuzzleLevelButton.puzzleLevelOffset + advanceNum && advanceNum > 0){
			Destroy(gameObject);
		}
	} 

	void Update () {
	}

	public void ChangeOffset() {
		PuzzleLevelButton.puzzleLevelOffset += advanceNum;
		Application.LoadLevel(Application.loadedLevel);

	}
}
