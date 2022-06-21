using UnityEngine;
using System.Collections;

public class GuiPuzzle : MonoBehaviour {
	
	public GameObject failPanel;
	public GameObject successPanel;
	public GameObject manager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public void retryPuzzle(){
//		Debug.Log ("CLICKED");
//		failPanel.SetActive (false);
//		successPanel.SetActive (false);
//		GameManager.hasOverlay = false;
//		manager.GetComponent<NewPuzzleManager> ().updateLevelOnButton();
////		Application.LoadLevel("PuzzleScreen");
//	}
}
