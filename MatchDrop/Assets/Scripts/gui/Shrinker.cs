using UnityEngine;
using System.Collections;

public class Shrinker : MonoBehaviour {
	
	Vector3 startSize;
	Vector3 destSize;

	float lerpPer;
	public float moveAmt = 3f;

	// Use this for initialization
	void Start () {
		startSize = transform.localScale;
		destSize = new Vector3(0, 0, 0);
		lerpPer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		lerpPer += moveAmt * Time.deltaTime;
		
		if(lerpPer >= 1){
			lerpPer = 1;
		}
		
		transform.localScale = Vector3.Lerp(startSize, destSize, lerpPer);
	}
}
