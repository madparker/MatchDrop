using UnityEngine;
using System.Collections;

public class PointAnim : MonoBehaviour {
	
//	float pixelsToUnits = 100;
//	float halfHeight = Screen.height * 0.5f;

	private float num = 1;
	private const float moveAmt = 1.1f;

	// Use this for initialization
	void Start () {
		num = 0;

//		GetComponent<MeshRenderer>().sortingLayerID = 1;
		GetComponent<MeshRenderer>().sortingOrder = 1;
	}

	public void Reset(){
		num = 0;
	}
	
	// Update is called once per frame
	void Update () {
		num += moveAmt * Time.deltaTime;

		float scale = Mathf.Sin(num * Mathf.PI) * .175f;// * 40f;
		
		TextMesh text = GetComponent<TextMesh>();
		//text.fontSize = (int)((halfHeight/pixelsToUnits) * scale);
		text.transform.localScale = new Vector3(scale, scale, scale);

		if(scale <= 0){
			ObjectPool.RemovePoint(gameObject);
		}
	}
}
