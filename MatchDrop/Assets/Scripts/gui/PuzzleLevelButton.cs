using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PuzzleLevelButton : MonoBehaviour {

	public int puzzleLockLevel;
	public int puzzleLevel;
	public int pLevelAndOffset;

	public static int puzzleLevelOffset = 0;

	public bool isChallenge = false;
	
	bool engage;
	
	// Use this for initialization
	void Start () {

		int maxLevel;

 		if(isChallenge){
			puzzleLockLevel = PlayerPrefs.GetInt(ChallengeManager.PREF_CHALLENGE_HIGH, 0);
			maxLevel = ChallengeManager.challenges.Length;
		} else {
			NewPuzzleManager.makeLevels();
			puzzleLockLevel = PlayerPrefs.GetInt(PuzzleManager.PREF_PUZZLE_HIGH, 0);
			maxLevel = NewPuzzleManager.levels.Length;
		}
		
		pLevelAndOffset = puzzleLevel + puzzleLevelOffset;

		transform.Find("LevelText").GetComponent<Text>().text = (pLevelAndOffset + 1) + "";

		if(puzzleLockLevel < pLevelAndOffset){
			transform.Find("Lock").gameObject.SetActive(true);
			transform.Find("CheckBox").gameObject.SetActive(false);
			transform.Find("CheckMark").gameObject.SetActive(false);
		} else if(puzzleLockLevel == pLevelAndOffset){
			transform.Find("Lock").gameObject.SetActive(false);
			transform.Find("CheckBox").gameObject.SetActive(true);
			transform.Find("CheckMark").gameObject.SetActive(false);
		} else {
			transform.Find("Lock").gameObject.SetActive(false);
			transform.Find("CheckBox").gameObject.SetActive(true);
			transform.Find("CheckMark").gameObject.SetActive(true);
		}

		GetComponent<ChangeScreenButtonLock>().lockLevel = pLevelAndOffset;

		if(maxLevel <= pLevelAndOffset){
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	public void SelectLevel()
	{
		if(isChallenge){
			PlayerPrefs.SetInt(ChallengeManager.PREF_CHALLENGE_LEVEL, pLevelAndOffset);
		} else {
			PlayerPrefs.SetInt(PuzzleManager.PREF_PUZZLE_LEVEL, pLevelAndOffset);
		}
	}
}