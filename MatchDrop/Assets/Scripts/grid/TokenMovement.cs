using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TokenMovement  : MonoBehaviour
{	//96 + 71 = 167
	//152 
	
	bool inMove = false;
	public static float lerpPer = 0;
	
	Vector3 startPos;
	public Vector3 destPos;

	const float MOVE_ACCELL = 5.0f;
	const float MIN_SPEED = 2.5f;
	const float MAX_SPEED = 7.5f;
	static float speed = MIN_SPEED;
	
	public static bool inAnim = false;
	
	PositionMessage gridPosition;

	static List<TokenMovement> moveList = new List<TokenMovement>();

	public void Duplicate(TokenMovement tm){
		inMove = tm.inMove;
		startPos = tm.startPos;
		destPos = tm.destPos;
		gridPosition = tm.gridPosition;
	}

	// Use this for initialization
	void Start ()
	{

		Vector3 pos = transform.position;
		
		startPos = new Vector3 (pos.x, pos.y, 0);
		//destPos = new Vector3 (pos.x, pos.y, 0);
	}

	public static void updateTokens(){
		if(lerpPer < 1){
			GameManager.fallingTokens = true;
			
			speed += MOVE_ACCELL * Time.deltaTime;
			
			if(speed > MAX_SPEED){
				speed = MAX_SPEED;
			}


			lerpPer += speed * Time.deltaTime;

			if(lerpPer >= 1){
				speed = MIN_SPEED;
				lerpPer = 1;
				inAnim = false; //TODO, this is the problem, inAnim can change before
				//anim actually completes, I think
				GridHandler.next = null;
				GameManager.fallingTokens = false;
				finalMoves();	
			}
		} else {
//			speed = MIN_SPEED;
//			inAnim = false;
//			GameManager.fallingTokens = false;
		}
	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{

		if (inMove && lerpPer < 1) {	
			transform.localPosition = Vector3.Lerp (startPos, destPos, lerpPer);
		} else if (inMove){	
			inAnim = false;
			finalDest();
		}
	}

	public void finalDest(){
		inMove = false;
		transform.position = new Vector3(destPos.x, destPos.y, destPos.z);
		if (gridPosition != null) {
			gameObject.SendMessage ("SetGridPos", gridPosition, SendMessageOptions.DontRequireReceiver);
			gridPosition = null;
		}
	}

	public static void finalMoves(){

		foreach(TokenMovement tm in moveList){
			if(tm != null){
				tm.finalDest();
			}
		}
		
		moveList.Clear();

//		for (int i = 0; i < moves.Length; i++) {
//			if(moves[i] != null){
////				Debug.Log (i);
//				moves[i].finalDest();
//				moves[i] = null;
//			}
//		}
	}
	
	public void setNewDestWithGridPos (PositionMessage pm)
	{		
		lerpPer = 0;
		setNewDest(pm.vec.x, pm.vec.y,  pm.vec.z);
		gridPosition = pm;
	}
	
	
	public void setNewDestWithGridPos (float x, float y, int gridX, int gridY)
	{
		setNewDest(x, y, 0);
		if(gridX >= 0){
			gridPosition = new PositionMessage(gridX, gridY);
		}
	}	
	
	public void setNewDest (float x, float y, float z)
	{		
		Vector3 position = transform.position;
		
		startPos = new Vector3 (position.x, position.y, position.z);
		
		destPos = new Vector3 (x, y, z);
		
		if(!startPos.Equals(destPos)){
			lerpPer = 0;
			inMove = true;
			inAnim = true;
			moveList.Add(this);
		} else {
			transform.position = destPos;
		}
		
		gridPosition = null;
	}

	public void moveToken(float x, float y){
		transform.position = new Vector3 (x, y, 0);
		startPos = new Vector3 (x, y, 0);
		destPos = new Vector3 (x, y, 0);
	}
	
	public void gridRestructNewDest (PositionMessage pm)
	{
		gameObject.SendMessage("SetGridPos", new PositionMessage(pm.gridX, pm.gridY), SendMessageOptions.RequireReceiver);
		setNewDest(pm.vec.x, pm.vec.y, pm.vec.z);
		gridPosition = null;
	}
	
	public bool atDestination ()
	{
		return transform.position.Equals(destPos);
	}
}