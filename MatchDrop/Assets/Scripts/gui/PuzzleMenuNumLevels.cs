using UnityEngine;
using System.Collections;

public class PuzzleMenuNumLevels : MonoBehaviour {

	// Use this for initialization
	void Awake () {
//		PuzzleLevelButton.puzzleLevelOffset = PlayerPrefs.GetInt (PuzzleManager.PREF_PUZZLE_LEVEL, 0);
//		PuzzleLevelButton.puzzleLevelOffset = PuzzleLevelButton.puzzleLevelOffset - (PuzzleLevelButton.puzzleLevelOffset%9);
		PuzzleManager.totalLevels = PuzzleManager.levels.Length;
			
	}

	// Update is called once per frame
	void Update () {
	
	}
}
