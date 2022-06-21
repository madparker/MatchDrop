using UnityEngine;
using System.Collections;

public class LerpMoveDot : MonoBehaviour {

	public Vector3 start;
	public Vector3 dest;

	float lerpPer;
	private const float moveAmt = 2f;

	// Use this for initialization
	void Start () {
		lerpPer = 0;
	}

	public void SetUp(Vector3 pos1, Vector3 pos2){
		start = pos1;
		dest = pos2;

		transform.position = pos1;
	}

	void Update () {

		if(HintHelper.slowAnim){
			lerpPer += moveAmt/3 * Time.deltaTime;
		} else {
			lerpPer += moveAmt * Time.deltaTime;
		}

		if(lerpPer >= 1){
			lerpPer = 1;

			NextTokenHandler.AddPowerUps();

			Destroy(gameObject);
		}

		transform.position = Vector3.Lerp(start, dest, lerpPer);

		if(HintHelper.slowAnim){
			HintHelper.instance.SetMatchHighLight(transform.position);
		}
	}
}
