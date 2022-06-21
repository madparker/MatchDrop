using UnityEngine;
using System.Collections;

public class GuiChallenge : MonoBehaviour {
	
	public GameObject panel;
	public GameObject manager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void retryChallenge(){
		Debug.Log ("CLICKED");
		panel.SetActive (false);
		GameManager.hasOverlay = false;
		manager.GetComponent<ChallengeManager> ().Setup ();
//		Application.LoadLevel("PuzzleScreen");
	}
}
