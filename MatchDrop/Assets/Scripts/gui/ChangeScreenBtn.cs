using UnityEngine;
using System.Collections;

public class ChangeScreenBtn : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ChangeScreen (string screenName) {
		Application.LoadLevel(screenName);
	}
}