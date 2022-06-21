using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NextTokenHandler : MonoBehaviour {
	public static List<TokenMovement>nextTokens;//Token
	public static List<GridToken.POWERS> powerUps;//PoweUps
//	public static GridHandler gridHandler;

	protected float offset;

	public static List<int> bag;

	static int NUM_PER_TOKEN = 3;
	
	public int selectIndex = -1;
	public TokenMovement selectedToken = null;

	public virtual void Setup () {
		GridHandler.Init();
		
		powerUps = new List<GridToken.POWERS>();
		nextTokens = new List<TokenMovement>();
		bag = new List<int>();
		
		offset = -GridHandler.GRID_SIZE;
		
		for(int i = 0; i < GridHandler.ROW_UP_NUM; i++){
			GameObject bt = (GameObject)Instantiate(Resources.Load("Prefabs/Token/BlankToken", typeof(GameObject)));
			nextTokens.Add(bt.GetComponent<TokenMovement>());	
			bt.GetComponent<TokenMovement>().moveToken(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[i] + offset);
			bt.transform.parent = this.transform;		
		}
		
		GameObject rut = (GameObject)Instantiate(Resources.Load("Prefabs/Token/RowUpToken", typeof(GameObject)));
		nextTokens.Add(rut.GetComponent<TokenMovement>());

		rut.GetComponent<TokenMovement>().moveToken(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[GridHandler.GRID_HEIGHT - 1]);	
		
		rut.transform.parent = this.transform;	

//		rut.transform.position = new Vector3 (GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[GridHandler.GRID_HEIGHT - 1], 0);
//		
//		rut.transform.parent = this.transform;	
	}

	public static int GetNextFromBag(){
		if (bag.Count == 0) {
			BrandNewBag();
		}

		int result = bag [0];

		NextTokenHandler.bag.RemoveAt(0);

		return result;
	}

	static void BrandNewBag(){
		bag.Clear();
		for (int type = 0; type < DisplayToken.MAX_TYPE; type++) { //LOOP through types
			for (int tokenNum = 0; tokenNum < NUM_PER_TOKEN; tokenNum++) { //Reap NUM_PER_TOKEN times
				//Insert "type" into random position "NUM_PER_TOKEN" times
				bag.Insert((int)Random.Range(0, bag.Count), type); 
			}		
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(nextTokens.Count > 0){
			TokenMovement topToken = (TokenMovement)nextTokens[0];

			if (topToken.GetComponent<BlankTokenDisplay> () != null) {
				topToken.GetComponent<BlankTokenDisplay>().Replace( GridToken.POWERS.POWER_NONE, 0);
			}
		}

	}

	public void GetSelection(Vector3 vec){
		selectIndex = GetIndex(Camera.main.ScreenToWorldPoint(vec).y);
		if(selectIndex != -1){
			selectedToken = nextTokens[selectIndex];
		}
	}

	public bool InPrevRange(Vector3 vec){
		return 
			Camera.main.ScreenToWorldPoint(vec).x <= GridHandler.cols [0] - DisplayToken.startSize * .75f &&
			Camera.main.ScreenToWorldPoint (vec).y < GridHandler.rows [0] + DisplayToken.startSize * 2;
	}

	public int GetIndex(float yPos){
		int index = -1;

		float margin = DisplayToken.startSize * .75f;

		for(int i = 0; i < nextTokens.Count; i++){
			if(i == 8){
				if(GridHandler.rows [GridHandler.rows.Length - 1] - margin < yPos){
					index = 8;
					break;
				}
			} else if(GridHandler.rows [i] + margin < yPos){
				index = i;
				break;
			}
		}

//		Debug.Log (index);

		if (index != -1 && nextTokens [index].GetComponent<BlankTokenDisplay> () == null) {
			return index;
		} else{
			return -1;
		}
	}

	public void InsertSelected(){
	
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
			result.setNewDest(GridHandler.cols[GridHandler.cols.Length/2], GridHandler.rows[0] + offset, 0);
			result.transform.parent = GameManager.getGameManager().transform;

			if(result.GetComponent<BlankTokenDisplay>() != null){
				result.GetComponent<BlankTokenDisplay> ().Replace();
			}

			if(!GameManager.touchDevice){
				Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, GridHandler.rows[0] + offset, 0));

				if(pos.x > GridHandler.cols[GridHandler.cols.Length - 1]){
					pos.x = GridHandler.cols[GridHandler.cols.Length - 1];
				}
				pos.y = GridHandler.rows[0] + offset;
				result.setNewDest(pos.x, pos.y, 0);
			}
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
		
		if(!GridHandler.gameOver){
			GameObject bt = (GameObject)Instantiate (Resources.Load ("Prefabs/Token/BlankToken", typeof(GameObject)));
			nextTokens.Add (bt.GetComponent<TokenMovement> ());

			bt.GetComponent<TokenMovement> ().moveToken (GridHandler.cols [0] - GridHandler.GRID_SIZE, GridHandler.rows [GridHandler.rows.Length - 1]);
			bt.transform.parent = this.transform;		
		}
	}
	
	public void addLevelUpToken(){
		
//		GameManager.next = getNextToken();

		int lastIndex = nextTokens.Count - 1;
		
		TokenMovement oldToken = (TokenMovement)nextTokens[lastIndex];
		nextTokens.RemoveAt(lastIndex);
		Destroy(oldToken.gameObject);
		
		GameObject rut = (GameObject)Instantiate (Resources.Load("Prefabs/Token/RowUpToken", typeof(GameObject)));
		nextTokens.Add(rut.GetComponent<TokenMovement>());

		
		rut.transform.parent = this.transform;	
		rut.GetComponent<TokenMovement>().moveToken(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[GridHandler.rows.Length - 1]);	
	}

	public TokenMovement getNextBlank(){
		int i = getNextBlankIndex();
		
		if(i >= 0){
			return (TokenMovement)nextTokens[getNextBlankIndex()];
		} else {
			return null;
		}
	}

	public Vector3 getNextBlankPos(){
		int i = getNextBlankIndex();

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
	
	public static int getNextBlankIndex(){

		int nextAvailableToken = -1;
		
		for(int i = 0; i < nextTokens.Count; i++){
			BlankTokenDisplay btd = nextTokens[i].GetComponent<BlankTokenDisplay>();
			if(btd != null && !btd.inReplace){
				nextAvailableToken = i;
				break;
			}
		}
		
		return nextAvailableToken;
	}
	
	public static void setNextToken(GridToken.POWERS powerUp){
		int nextAvailableToken = getNextBlankIndex();

		if(nextAvailableToken < 0){
			GameManager.score += nextTokens.Count * 10;
		} else {
			TokenMovement oldToken = (TokenMovement)nextTokens[nextAvailableToken];
//			GameObject newToken;
//
//			switch (powerUp) {
//			case GridToken.POWERS.POWER_COLOR_ZAP:
//				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenZap", typeof(GameObject)));
//				break;
			//			case GridToken.POWERS.POWER_BOMB:GridToken.POWERS powerUp
//				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
//				break;
//			case GridToken.POWERS.POWER_MULT_X1:
//				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/MultiToken", typeof(GameObject)));
//				break;
//			default:
//				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));
//				break;
//			}
//				
//			newToken.GetComponent<TokenMovement>().moveToken(oldToken.transform.position.x, oldToken.transform.position.y);
//			newToken.transform.parent = oldToken.transform.parent;
//
//			nextTokens.RemoveAt(nextAvailableToken);
//			nextTokens.Insert(nextAvailableToken, newToken.GetComponent<TokenMovement>());

			oldToken.GetComponent<BlankTokenDisplay>().Replace(powerUp, nextAvailableToken);
//			Destroy(oldToken.gameObject);
		}
	}
}
