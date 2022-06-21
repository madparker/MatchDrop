using UnityEngine;
using System.Collections;

public class GuiTutorial : MonoBehaviour {
	
	public GameObject panel;
	public GameObject manager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void retryTutorial(){
		panel.SetActive (false);
		GameManager.hasOverlay = false;
		manager.GetComponent<TutorialManager> ().updateLevel ();
		Debug.Log ("CLICKED");
	}
}
