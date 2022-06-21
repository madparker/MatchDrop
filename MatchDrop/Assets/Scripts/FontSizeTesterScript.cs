using UnityEngine;
using System.Collections;

public class FontSizeTesterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GameObject textHolder = new GameObject("textHolder1");
		textHolder.transform.parent = transform;
		
		TextMesh text = textHolder.AddComponent<TextMesh>();
		text.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		text.font = Resources.Load<Font>("Fonts/BradBunR");
		text.fontSize = 113;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
