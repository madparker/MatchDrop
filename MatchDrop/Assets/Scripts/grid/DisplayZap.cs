using UnityEngine;
using System.Collections;

public class DisplayZap : DisplayToken {

	LineRenderer lineRenderer;
	
	int numPointsInLine = 1000;
	
	float width = 0.02f;
	
	float maxMod = .75f;

	// Use this for initialization
	void Start () {
		type = 5;

		lineRenderer = gameObject.GetComponent<LineRenderer> ();
		
		lineRenderer.SetVertexCount (numPointsInLine);
		
		lineRenderer.SetWidth(width, width); 
		
		float a = (Mathf.PI * 2)/(float)(numPointsInLine - 1);
		
		for (int i = 0; i < numPointsInLine; i++) {
			
			Vector3 pos = new Vector3(Mathf.Sin (a * i) * maxMod, Mathf.Cos (a * i) * maxMod, 1);
			
			lineRenderer.SetPosition(i, pos);
		}
	}

	public override void Display () {
		base.Display();
		float mod = 0;
		
		float a = (Mathf.PI * 2)/(float)(numPointsInLine/8f);

		for (int i = 0 ; i < numPointsInLine; i++) {
			mod += 0.0005f;

			if(maxMod < mod){
				mod = maxMod;
			}
			//if(i%4 != 0){	
			Vector3 pos = new Vector3(Mathf.Sin (a * i) * mod + Random.Range(-0.1f, 0.1f), 
			                          Mathf.Cos (a * i) * mod + Random.Range(-0.1f, 0.1f), -0.1f);
			lineRenderer.SetPosition(i, pos);
			//}
		}
	}
}
