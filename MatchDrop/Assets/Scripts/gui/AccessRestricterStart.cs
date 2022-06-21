using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccessRestricterStart : AccessRestricter {

	public Sprite lockImage;

	public override void UpdateToLockedDisplay(GameObject button){
		button.transform.Find("Image").GetComponent<Image>().sprite = lockImage;
	}

	public override int GetLockLevel(){

		return PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_INTRO);
	}

}
