using UnityEngine;
using System.Collections;

public class GridToken : MonoBehaviour {

	public static int currentStreak = 0;

	//FINAL VARS
	public enum DIRS{
		DIR_HORZ = 0,
		DIR_VERT = 1,
		DIR_DIAG_UP = 2, 
		DIR_DIAG_DN = 3
	};
	
	public enum POWERS{
		POWER_ROW_UP = -3, POWER_BLANK = -2, POWER_COLOR_ZAP = -1, 
		POWER_NONE = 0,
		POWER_MULT_X1 = 1,
		POWER_BOMB = 2,
	};

	public int gen = 0;

	public int gridX = -1;
	public int gridY = -1;
	
	public bool marked = false;
	
	public static float TOKEN_SIZE = 110;
	public static float BORDER_SIZE = 10;
	public static float GRID_SIZE = TOKEN_SIZE + BORDER_SIZE;
	public static bool isInit = false;
	public static GridToken[,] grid;
	public static GridHandler gridHandler;
	public static NextTokenHandler nextHandler;
	
	public int[] streak = new int[4];
	
	public DisplayToken tokenDisplay;
	
	public static GameObject startStreak;
	public static GameObject endStreak;
	
	// Use this for initialization
	void Awake () {

		streak = new int[4];
		gridX = -1;
		gridY = -1;
		tokenDisplay = gameObject.GetComponent<DisplayToken>();
	}
	
	// Update is called once per frame
	void Update () {
		if(tokenDisplay == null){
			tokenDisplay = gameObject.GetComponent<DisplayToken>();
		}

		if(gridX != -1 && gridY != -1 ){
			if(grid[gridX, gridY] != this){
				grid[gridX, gridY] = this;
				Debug.Log("GEN: " + gen);
				throw new System.Exception("Die, fiend!");
			}
		}
	}
	
	public void SetGridPos(PositionMessage gridPos){
		if(gridPos.gridX >= 0){
			if(gridX >= 0){
				grid[gridX, gridY] = null;
			}
				
			gridX = gridPos.gridX;		
			gridY = gridPos.gridY;

			if(gridY >= 0){
				grid[gridX, gridY] = this;
			}
			this.transform.parent = gridHandler.gameObject.transform;
		}
	}
	
	public void SetGridPosAndPos(PositionMessage gridPos){
		if(gridPos.gridX >= 0){
			if(gridX >= 0){
				grid[gridX, gridY] = null;
			}
				
			gridX = gridPos.gridX;		
			gridY = gridPos.gridY;
			
			grid[gridX, gridY] = this;
			this.transform.parent = gridHandler.gameObject.transform;
			
			transform.position = new Vector3(gridPos.vec.x, gridPos.vec.y, 0);
//			gameObject.GetComponent<TokenMovement>().justMove(gridPos.vec.x, gridPos.vec.y);
//			gameObject.GetComponent<TokenMovement>().destPos = new Vector3 (gridPos.vec.x, gridPos.vec.y, 0);
		}
	}
	
	public virtual bool isMatch (int otherType)
	{	
		return (tokenDisplay.type == otherType) && tokenDisplay.size > 0;
	}
	
	public virtual bool hasMatch (bool markMatched)
	{
		
		bool hasMatch = hasDirMatch (DIRS.DIR_HORZ, tokenDisplay.type, markMatched);
		
		hasMatch = hasDirMatch (DIRS.DIR_VERT, tokenDisplay.type, markMatched) || hasMatch;
		hasMatch = hasDirMatch (DIRS.DIR_DIAG_DN, tokenDisplay.type, markMatched) || hasMatch;
		hasMatch = hasDirMatch (DIRS.DIR_DIAG_UP, tokenDisplay.type, markMatched) || hasMatch;
		
		return hasMatch;
	}
	
	public virtual bool hasDirMatch (DIRS dir, int matchType, bool markMatched)
	{
		GridToken token = this;
		
		int streak = 0;
		
		while (token != null && token.isMatch(matchType) && isEdge(dir, streak)) {
			
			token = getNextToken(dir, streak);
	
			if (token != null) {
				if (token.isMatch (matchType)){
					streak++;
				}
			}
		}
		
		if (streak >= 3) {
		
			if(streak > currentStreak){
				currentStreak = streak;
			}
			
			//Debug.Log("STreak: " + streak);
			if(markMatched && !marked){
	
				GameManager.getGameManager ().GetComponent<GameManager> ().playMatchSound();


				for (int i = 0; i < streak; i++) {
					
					token = getNextToken (dir, i);
							
					if (token.streak [(int)dir] < streak) {
						token.streak [(int)dir] = streak;
					}
				}

				if(streak == currentStreak){
					GameObject line = ObjectPool.GetLine();
					startStreak = this.gameObject;
					endStreak =  getNextToken(dir, streak - 1).gameObject;

					line.GetComponent<Line>().SetUp(transform.position, endStreak.transform.position, 10);
				}
			}

			return true;
		} else {
			return false;
		}
	}
	
	public virtual void markToken ()
	{
		if (!marked) {
			DisplayToken.inAnim = true;
			marked = true;

			if (GameManager.unlockPrev && GetComponent<DisplayRockToken>() == null) {
				GameObject dot = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/Rect", typeof(GameObject)));

				dot.GetComponent<Rectangle> ().SetUp (0.25f, 0.25f);

				dot.GetComponent<LerpMoveDot> ().SetUp (
					transform.position, 
					nextHandler.getNextBlankPos ());
				
				GameObject points = ObjectPool.GetPoint();
				points.transform.position = new Vector3 (transform.position.x,
	                        transform.position.y,
	                        transform.position.z + 2);
				GridHandler.pointDisplays.Add (points.GetComponent<TextMesh> ());
			}
		}
	}
	
	public GridToken getNextToken (DIRS dir, int streak)
	{
		
		GridToken token = null;
		
		switch (dir) {
		case DIRS.DIR_VERT:
			token = grid [gridX, gridY + streak];
			break;
		case DIRS.DIR_HORZ:
			token = grid [gridX + streak, gridY];
			break;
		case DIRS.DIR_DIAG_DN:
			token = grid [gridX - streak, gridY + streak];
			break;
		case DIRS.DIR_DIAG_UP:
			token = grid [gridX + streak, gridY + streak];
			break;
		}
		
		return token;
	}
	
	public bool isEdge (DIRS dir, int streak)
	{		
		//Debug.Log("GRIDX: " + gridX + " GRIDY: " + gridY);

		bool edge = false;
		
		switch (dir) {
		case DIRS.DIR_VERT:
			edge = gridY + streak < GridHandler.GRID_HEIGHT;
			break;
		case DIRS.DIR_HORZ:
			edge = gridX + streak < GridHandler.GRID_WIDTH;
			break;
		case DIRS.DIR_DIAG_DN:
			edge = 
				gridX - streak > -1 && 
				gridY + streak < GridHandler.GRID_HEIGHT;
			break;
		case DIRS.DIR_DIAG_UP:
			edge = 
				gridX + streak < GridHandler.GRID_WIDTH && 
				gridY + streak < GridHandler.GRID_HEIGHT;
			break;
		}
		
		return edge;
	}

	public int getTotalMatches(){
		int total = 0;

		for(int i = 0; i < 4; i++) {
			total += streak [i];
		}

		return total;
	}
	
	public void remove(){
		GridToken.currentStreak = 0;
		
		if(tokenDisplay.type >= 0  && tokenDisplay.type < DisplayToken.sprites.Length){
			GameManager.tokensRemoved[tokenDisplay.type]++;
			if(GetComponent<DisplayMultiToken>() != null){
				GameManager.tokensRemoved[GetComponent<DisplayMultiToken>().type2]++;
				GameManager.powerUpsRemoved[ChallengeManager.POWERUP_MULTI]++;
			}
			if(GetComponent<DisplayBombToken>() != null){
				GameManager.powerUpsRemoved[ChallengeManager.POWERUP_BOMB]++;
			}
		}

		if(GetComponent<DisplayZap>() != null){
			GameManager.powerUpsRemoved[ChallengeManager.POWERUP_ZAP]++;
		}

		grid[gridX, gridY] = null;
		gridX = -1;
		gridY = -1;
		GameManager.updateScoreLevelDelegate();

		for(int i = 0; i < streak.Length; i++){
			streak[i] = 0;
		}

//		foreach (Transform child in transform){
//			Destroy (child.gameObject);
//		}

		GridHandler.removeHoles();

		ObjectPool.RemoveToken(this.gameObject);
	}

	void OnDestroy() {
//		print("Script was destroyed");
	}
}
