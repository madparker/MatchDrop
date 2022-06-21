using UnityEngine;
using System.Collections;

public class GuiEndless : MonoBehaviour {
	
	public GameObject panel;
	public GameObject manager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void restartGame(){
		panel.SetActive (false);
		GameManager.hasOverlay = false;
		Application.LoadLevel("GameEndless");
	}

	public void saveToken(){

		GameObject saveToken = GameObject.Find ("SaveToken");
		GameObject next = GameManager.Next;

		Vector3 nextPos = Util.CloneVector3 (next.transform.position);

		GameManager.Next = saveToken;
		next.name = "SaveToken";
		next.transform.parent = null;
		next.transform.position = Util.CloneVector3 (saveToken.transform.position);

		saveToken.name = "next";
		saveToken.transform.position = nextPos;
		saveToken.transform.parent = GameManager.getGameManager().transform;
	}
}
