using UnityEngine;
using System.Collections;

public class FocusAreaScript : MonoBehaviour {
	
	public const string HL_TYPE_OFF = "off";
	public const string HL_TYPE_GRID = "grid";
	public const string HL_TYPE_PREV = "prev";
	public const string HL_TYPE_SWAP = "swap";
	public const string HL_TYPE_MATCH = "match";
	public const string HL_TYPE_MATCH_PREV = "matchPrev";
	public const string HL_TYPE_MATCH_PREV2 = "matchPrev2";
	
	const string SHADER_START = "_Vector1";
	const string SHADER_END   = "_Vector2";
	const string SHADER_PREV  = "_Vector3";
	const string SHADER_MODE = "_Mode";

	public Vector3 startPoint;
	public Vector3 endPoint;
	public Vector3 prevPoint;
	
	Material mat;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetHighLightType(string type){

		if(mat == null){
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			mat = sr.material;
		}

//		Debug.Log("type: " + type);

		if(HL_TYPE_OFF.Equals(type)){
			GetComponent<SpriteRenderer>().enabled = false;
		} else if(HL_TYPE_GRID.Equals(type)){
			GetComponent<SpriteRenderer>().enabled = true;
			mat.SetVector(SHADER_START,   
			              new Vector3(
							GridHandler.cols[GridHandler.cols.Length - 1] + GridHandler.GRID_SIZE/2,
							GridHandler.rows[0] - GridHandler.GRID_SIZE/2));
			mat.SetVector(SHADER_END, 
			              new Vector3(
							GridHandler.cols[0] - GridHandler.GRID_SIZE/2, 
			                GridHandler.rows[GridHandler.rows.Length - 1] + GridHandler.GRID_SIZE/2));
			mat.SetInt(SHADER_MODE,  1);
		} else if(HL_TYPE_PREV.Equals(type)){
			GetComponent<SpriteRenderer>().enabled = true;
			
			Vector3 start = Util.CloneVector3(NextTokenHandler.nextTokens[0].gameObject.transform.position);
			start.x += GridHandler.GRID_SIZE/2;
			start.y -= GridHandler.GRID_SIZE/2;
			Vector3 end = Util.CloneVector3(NextTokenHandler.nextTokens[GridHandler.ROW_UP_NUM].gameObject.transform.position);
			end.x -= GridHandler.GRID_SIZE/2;
			end.y += GridHandler.GRID_SIZE/2;
			
			mat.SetVector(SHADER_START, start);
			mat.SetVector(SHADER_END, end);
			mat.SetInt(SHADER_MODE,  1);
		} else if(HL_TYPE_SWAP.Equals(type)){
			GetComponent<SpriteRenderer>().enabled = true;
			
			Vector3 start = GameObject.Find ("SaveToken").transform.position;
			Vector3 end = Util.CloneVector3(start);
			start.x += GridHandler.GRID_SIZE * 0.75f;
			start.y -= GridHandler.GRID_SIZE * 0.75f;
			end.x -= GridHandler.GRID_SIZE * 0.75f;
			end.y += GridHandler.GRID_SIZE * 0.75f;
			
			mat.SetVector(SHADER_START, start);
			mat.SetVector(SHADER_END, end);
			mat.SetInt(SHADER_MODE,  1);
		} else if (HL_TYPE_MATCH.Equals(type)){
			GetComponent<SpriteRenderer>().enabled = true;
			mat.SetVector(SHADER_START, startPoint);
			mat.SetVector(SHADER_END,   endPoint);
			mat.SetVector(SHADER_PREV,  prevPoint);
			mat.SetInt(SHADER_MODE,  2);
		} else if (HL_TYPE_MATCH_PREV.Equals(type)){
			GetComponent<SpriteRenderer>().enabled = true;
			mat.SetVector(SHADER_START, startPoint);
			mat.SetVector(SHADER_END,   endPoint);
			mat.SetVector(SHADER_PREV,  prevPoint);
			mat.SetInt(SHADER_MODE,  3);
		} else if (HL_TYPE_MATCH_PREV2.Equals(type)){
			prevPoint.y += GridHandler.GRID_SIZE/2;

			GetComponent<SpriteRenderer>().enabled = true;
			mat.SetVector(SHADER_START, startPoint);
			mat.SetVector(SHADER_END,   endPoint);
			mat.SetVector(SHADER_PREV,  prevPoint);
			mat.SetInt(SHADER_MODE,  4);
		}
	}
}
