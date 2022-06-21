using UnityEngine;
using System.Collections;

public class ChallengeMenuNumLevels : MonoBehaviour {

	// Use this for initialization
	void Awake () {
//		PuzzleLevelButton.puzzleLevelOffset = PlayerPrefs.GetInt(ChallengeManager.PREF_CHALLENGE_HIGH, 0);
//		PuzzleLevelButton.puzzleLevelOffset = PuzzleLevelButton.puzzleLevelOffset - (PuzzleLevelButton.puzzleLevelOffset%9);
		PuzzleManager.totalLevels = ChallengeManager.challenges.GetLength(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
