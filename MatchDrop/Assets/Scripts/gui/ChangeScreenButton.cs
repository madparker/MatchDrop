using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeScreenButton : MonoBehaviour {

	public string sceneName;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void ChangeToScreen(){
		Application.LoadLevel(sceneName);
	}
}
