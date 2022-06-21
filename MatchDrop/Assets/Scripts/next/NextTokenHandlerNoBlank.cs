using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NextTokenHandlerNoBlank : MonoBehaviour {
	public static List<TokenMovement>nextTokens;//Token
	public static List<GridToken.POWERS> powerUps;//PoweUps
//	public static GridHandler gridHandler;

	protected float offset;

	public virtual void Setup () {
		GridHandler.Init();
		
		powerUps = new List<GridToken.POWERS>();
		nextTokens = new List<TokenMovement>();
		
		offset = GridHandler.GRID_SIZE;
		
		for(int i = 0; i < GridHandler.ROW_UP_NUM; i++){
			GameObject bt = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));
//			GameObject bt = (GameObject)Instantiate(Resources.Load("Prefabs/Token/BlankToken", typeof(GameObject)));
			nextTokens.Add(bt.GetComponent<TokenMovement>());
			bt.transform.parent = this.transform;
			Debug.Log ("EREER");
			bt.GetComponent<TokenMovement>().moveToken(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[i] + offset);
//			bt.transform.position = new Vector3(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[i] + offset, 0); 
		}
		
		GameObject rut = (GameObject)Instantiate(Resources.Load("Prefabs/Token/RowUpToken", typeof(GameObject)));
		nextTokens.Add(rut.GetComponent<TokenMovement>());
		
		rut.transform.position = new Vector3 (GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[GridHandler.GRID_HEIGHT - 1], 0);
		
		rut.transform.parent = this.transform;	
	}
	
	// Update is called once per frame
	void Update () {
		foreach(TokenMovement token in nextTokens){
			token.gameObject.transform.parent = transform;
		}

		//Debug.Log("nextTokens: " + nextTokens.Count);

		/*if(powerUps.Count > 0){
			foreach(GridToken.POWERS power in powerUps){
				setNextToken(power);
			}
			
			powerUps.Clear();
		}*/
	}

	public static void AddPowerUps(){
		if(powerUps.Count > 0){
			foreach(GridToken.POWERS power in powerUps){
				setNextToken(power);
			}
			
			powerUps.Clear();
		}
	}

	public int getCount(){
		return nextTokens.Count;
	}
	
	public virtual GameObject getNextToken(){

		TokenMovement result = (TokenMovement)nextTokens[0];
		nextTokens.RemoveAt(0);
		
		bool isRowUp = false;
		
		if(result.GetComponent<RowUpToken>() != null){
			result.GetComponent<RowUpToken>().Step();
			isRowUp = true;
		}

		Repopulate();

		if(!isRowUp){
			result.setNewDestWithGridPos(new PositionMessage(new Vector2(GridHandler.cols[GridHandler.cols.Length/2], GridHandler.rows[0] + offset)));
		}
		
		for(int i = 0; i < GridHandler.rows.Length && i < nextTokens.Count; i++){
			TokenMovement token = (TokenMovement)nextTokens[i];
			Vector3 pos = token.gameObject.transform.position;
			token.gameObject.transform.position = new Vector3(pos.x, pos.y, 0);
			token.setNewDestWithGridPos(new PositionMessage(new Vector2(token.gameObject.transform.position.x, GridHandler.rows[i] + offset)));
		}

		return result.gameObject;
	}

	public virtual void Repopulate(){
//		GameObject bt = (GameObject)Instantiate (Resources.Load("Prefabs/Token/BlankToken", typeof(GameObject)));
		GameObject bt = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));
		bt.transform.position = new Vector3(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[GridHandler.rows.Length - 1], 1);
		nextTokens.Add(bt.GetComponent<TokenMovement>());
		bt.transform.parent = this.transform;
	}
	
	public void addLevelUpToken(){
		
		GameManager.Next = getNextToken();

		int lastIndex = nextTokens.Count - 1;
		
		TokenMovement oldToken = (TokenMovement)nextTokens[lastIndex];
		nextTokens.RemoveAt(lastIndex);
		Destroy(oldToken.gameObject);
		
		GameObject rut = (GameObject)Instantiate (Resources.Load("Prefabs/Token/RowUpToken", typeof(GameObject)));
		rut.transform.position = new Vector3(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[GridHandler.rows.Length - 1], 2);
		nextTokens.Add(rut.GetComponent<TokenMovement>());
		rut.transform.parent = this.transform;
	}

	public TokenMovement getNextBlank(){
		int i = getNextNonPoweredIndex();
		
		if(i >= 0){
			return (TokenMovement)nextTokens[getNextNonPoweredIndex()];
		} else {
			return null;
		}
	}

	public Vector3 getNextNonPoweredToken(){
		int i = getNextNonPoweredIndex();

		Vector3 result;

		if(i > -1){
			result = nextTokens[i].transform.position;
		} else {
			result = new Vector3(GridHandler.cols[0] - GridHandler.GRID_SIZE, 
			                     GridHandler.rows[GridHandler.cols.Length - 1] - GridHandler.GRID_SIZE,
			                     2);
		}

		return result;
	}
	
	public static int getNextNonPoweredIndex(){

		int nextAvailableToken = -1;
		
		for(int i = 0; i < nextTokens.Count; i++){

			DisplayToken displayToken = nextTokens[i].GetComponent<DisplayToken>();

			if(displayToken != null){
				string name = displayToken.GetType().Name;

				if(name.Equals("DisplayToken")){

					nextAvailableToken = i;
					break;
				}
			}
		}
		
		return nextAvailableToken;
	}
	
	public static void setNextToken(GridToken.POWERS powerUp){
		int nextAvailableToken = getNextNonPoweredIndex();

		if(nextAvailableToken < 0){
			GameManager.score += nextTokens.Count * 10;
		} else {
			TokenMovement oldToken = (TokenMovement)nextTokens[nextAvailableToken];
			GameObject newToken = null;

			switch (powerUp) {
			case GridToken.POWERS.POWER_COLOR_ZAP:
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenZap", typeof(GameObject)));
				break;
			case GridToken.POWERS.POWER_BOMB:
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
				break;
			case GridToken.POWERS.POWER_MULT_X1:
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/MultiToken", typeof(GameObject)));
				break;
			default:
//				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));
				break;
			}
			
			if(newToken != null){
				newToken.GetComponent<DisplayToken>().SetType(oldToken.gameObject.GetComponent<DisplayToken>().type);

			//Debug.Log("newToken: " + newToken.GetComponent<TokenMovement>());
				newToken.transform.position = oldToken.transform.position;
				
				/*
				newToken.setNewDest(oldToken.destPos.x, oldToken.destPos.y, -1, -1);
				
				oldToken = (TokenMovement)nextTokens[nextAvailableToken];
				*/
				//Debug.Log("nextTokens: size" + nextTokens.Count);

				nextTokens.RemoveAt(nextAvailableToken);
				nextTokens.Insert(nextAvailableToken, newToken.GetComponent<TokenMovement>());

				Destroy(oldToken.gameObject);
			}
		}
	}
}
