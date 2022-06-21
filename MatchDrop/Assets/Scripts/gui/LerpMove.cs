using UnityEngine;
using System.Collections;

public class LerpMove : MonoBehaviour {

	public bool inPos;
	public Vector3 start;
	public Vector3 dest;
	
	float lerpPer;
	public float moveAmt = 0.03f;
	
	// Use this for initialization
	void Start () {
		lerpPer = 0;

		if(start.Equals(dest)){
			inPos = true;
		} else {
			inPos = false;
		}
	}
	
	public void SetUp(Vector3 pos1, Vector3 pos2){
		start = pos1;
		dest = pos2;

		if(start.Equals(dest)){
			inPos = true;
			lerpPer = 1;
		} else {
			inPos = false;
			lerpPer = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		lerpPer += moveAmt * Time.deltaTime;
		
		if(lerpPer >= 1){
			lerpPer = 1;
			inPos = true;
		}
		
//		transform.position = Vector3.Lerp(start, dest, lerpPer);

	
	}
}
