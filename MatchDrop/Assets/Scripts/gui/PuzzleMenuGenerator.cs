using UnityEngine;
using System.Collections;

public class PuzzleMenuGenerator : MonoBehaviour {

	// Use this for initialization
	
	public int offsetX;
	public int offsetY;

	void Start () {
	
		for(int x = 0; x < 3; x++){
			for(int y = 0; y < 3; y++){
				GameObject button = Instantiate(Resources.Load("Prefabs/GUI/PuzzleModeButton"), new Vector3(offsetX, offsetY), Quaternion.identity) as GameObject;
				
				button.transform.localScale.Set(1, 1, 1);
				button.transform.parent = transform;
//				button.GetComponent<RectTransform>().position = new Vector3(offsetX, offsetY);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
