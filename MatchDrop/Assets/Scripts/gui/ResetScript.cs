using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour {
	
	void Start () {
	}
	
	void Update () {
	}


	public void ResetGame(){
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();

		Application.LoadLevel("StartScreen");
	}
}
