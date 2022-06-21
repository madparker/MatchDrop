using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccessRestricterPuzzle : AccessRestricter {

	public override void UpdateToLockedDisplay(GameObject button){

	}

	public override int GetLockLevel(){
		Debug.Log(PlayerPrefs.GetInt(NewPuzzleManager.PREF_PUZZLE_HIGH, 0));
		return PlayerPrefs.GetInt(NewPuzzleManager.PREF_PUZZLE_HIGH, 0);
	}

}
