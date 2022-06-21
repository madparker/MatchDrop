using UnityEngine;
using System.Collections;

public class DisplayBombToken : DisplayToken {

	LineRenderer lineRenderer;

	int numPointsInLine = 24;

	float width = 0.1f;

	float mod = 0.375f;

	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer> ();
		
		lineRenderer.SetVertexCount (numPointsInLine);
		
		lineRenderer.SetWidth(width, width); 

		Material mat = Resources.Load<Material>("Materials/lineMat");
		mat.color = new Color(10,10,50);	
		lineRenderer.material = mat;

		float a = (Mathf.PI * 2)/(float)(numPointsInLine - 1);

		for (int i = 0; i < numPointsInLine; i++) {
			
			Vector3 pos = new Vector3(Mathf.Sin (a * i) * mod + transform.position.x, 
			                          Mathf.Cos (a * i) * mod + transform.position.y, 
			                          0);
			lineRenderer.SetPosition(i, pos);
		}
		
		transform.Rotate (Vector3.down * Mathf.PI/4.5f * 400);
	}
	
	// Update is called once per frame
	public override void Display () {
		base.Display();
		UpdateRot ();

		if(gridToken.marked){
			mod = Mathf.Sin(startTime) * 1.15f;
		}

		float a = (Mathf.PI * 2)/(float)(numPointsInLine/2 - 1);

		for (int i = 0 ; i < numPointsInLine; i++) {
			Vector3 pos = new Vector3(Mathf.Sin (a * i) * mod + Random.Range(-0.04f, 0.04f) + transform.position.x, 
			                          Mathf.Cos (a * i) * mod + Random.Range(-0.04f, 0.04f) + transform.position.y, 
			                          0);
			lineRenderer.SetPosition(i, pos);

		}
	}
}
