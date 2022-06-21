using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
	
	LineRenderer lineRenderer;
	
	public int lengthOfLineRenderer = 0;
	public Vector3 pos1;
	public Vector3 pos2;
	
	public float width = 0.1f;
	
	Vector3 dot;

	float timer = 0.4f;
	
	// Use this for initialization
	void Start ()
	{	
		SetUp(pos1, pos2, lengthOfLineRenderer);
	}
	
	public void SetUp(Vector3 vec1, Vector3 vec2, int lgth){
		timer = 0.4f;

		pos1 = vec1;
		pos2 = vec2;

		lengthOfLineRenderer = lgth;
		
		lineRenderer = gameObject.GetComponent<LineRenderer> ();
		
		dot = Vector3.Cross(pos1 - pos2, Vector3.forward);
		
		lineRenderer.SetVertexCount (lengthOfLineRenderer);
		
		lineRenderer.SetWidth(width, width); 
		
		dot.Normalize();

		positionLine();
	}
	
	// Update is called once per frame
	void Update ()
	{	
		timer -= Time.deltaTime;

		if(timer <= 0){
			ObjectPool.RemoveLine(gameObject);
		}

		positionLine();
	}

	void positionLine(){
		for (int i = 0; i < lengthOfLineRenderer; i++) {
			
			Vector3 pos = Vector3.Lerp(pos1, pos2, i/((float)lengthOfLineRenderer - 1));
			
			pos += dot * 
				Random.Range(-width/2 * 2, width/2 * 2);
			
			
			lineRenderer.SetPosition(i, pos);
		}
	}
	
}

