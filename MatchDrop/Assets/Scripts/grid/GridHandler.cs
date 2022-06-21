using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridHandler : MonoBehaviour {
	
	private static bool isInit = false;
	
	public const int ROW_UP_NUM = 8;
	public static int newRow = ROW_UP_NUM;
	public const int GRID_WIDTH = 8;
	public const int GRID_HEIGHT = 8;
	public static float[] cols = new float[GRID_WIDTH];
	public static float[] rows = new float[GRID_HEIGHT];
	public static GridToken[,] grid = new GridToken[GRID_WIDTH, GRID_HEIGHT];
	public static Rect fieldRect = new Rect ();
	public static Color color;
	//public static NextTokenHandler nextHandler;
	public static RowUpToken rut;
	
	public const float TOKEN_SIZE = 5f/8.5f;
	public const float BORDER_SIZE = TOKEN_SIZE/10;
	public const float GRID_SIZE = TOKEN_SIZE + BORDER_SIZE;
	
	public static List<TextMesh> pointDisplays;//PoweUps
	
	public static List<GridRockToken> rocks;//PoweUps

	public AudioClip matchSound;

	Rectangle rect;

	public static TokenMovement next;

	public static int level = 1;

	bool isSet = false;

	bool hasLines = false;
	
	public static bool gameOver = false;
	
	public static void Init(){
		if(!isInit){
			pointDisplays = new List<TextMesh> ();

			rocks = new List<GridRockToken>();

			isInit = true;
			fieldRect.x = -2.385f;//Screen.width/ 2 - GridHandler.GRID_WIDTH / 2 * Token.GRID_SIZE;
			fieldRect.y = 2.25f;//Screen.height / 2 - GridHandler.GRID_HEIGHT / 2 * Token.GRID_SIZE;
	
			fieldRect.width = GRID_WIDTH * GRID_SIZE;
			fieldRect.height = GRID_HEIGHT * GRID_SIZE;
			
			color = Color.gray;
	
			for (int x = 0; x < GRID_WIDTH; x++) {
				cols [x] = fieldRect.x + x * GRID_SIZE + GRID_SIZE/2;
			}
	
			for (int y = 0; y < GRID_HEIGHT; y++) {
				rows [GRID_HEIGHT - y  - 1] = fieldRect.y - y * GRID_SIZE - GRID_SIZE/2;
			}
		}
	}
	
	void Awake ()
	{
		Init();

		gameOver = false;

		grid = new GridToken[GRID_WIDTH, GRID_HEIGHT];

		GridToken.grid = grid;
		GridToken.gridHandler = this;
		
		newRow = ROW_UP_NUM;
	}

	//NOTES
	//Don't distract player (even visually)
	//Limit choice space

	public void SetUpLines(){
		Material mat = Resources.Load<Material>("Materials/gridLineMat");
		mat.color = new Color(10,10,50);	

		MakeLine(-1, mat, 
		         cols[0] - GRID_SIZE/2, rows [0] - GRID_SIZE/2,
		         cols[0] - GRID_SIZE/2, rows [rows.Length - 1] + GRID_SIZE/2);
		MakeLine(-1, mat,
		         cols[0] - GRID_SIZE/2, rows [rows.Length - 1] + GRID_SIZE/2,
		         cols[cols.Length - 1] + GRID_SIZE/2, rows [rows.Length - 1] + GRID_SIZE/2);

		for (int i = 0; i < GRID_WIDTH; i++) {
			
			MakeLine(i, mat, 
			         cols[i] + GRID_SIZE/2, rows [0] - GRID_SIZE/2,
			         cols[i] + GRID_SIZE/2, rows [rows.Length - 1] + GRID_SIZE/2);
			MakeLine(i, mat,
			         cols[0] - GRID_SIZE/2, rows [i] - GRID_SIZE/2,
			         cols[rows.Length - 1] + GRID_SIZE/2, rows [i] - GRID_SIZE/2);
//			         cols[i] + GRID_SIZE/2, rows [rows.Length - 1] - GRID_SIZE/2);
		}

		level = 1;
	}

	public void MakeLine(int i, Material mat, float x1, float y1, float x2, float y2){			
		GameObject line = new GameObject("Line Column " + i);
		LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
		lineRenderer.SetVertexCount (2);
		lineRenderer.SetWidth(0.02f, 0.02f); 
		lineRenderer.SetPosition(0, new Vector3(x1, y1, 0));
		lineRenderer.SetPosition(1, new Vector3(x2, y2, 0));
		lineRenderer.material = mat;
		line.transform.parent = transform;
	}

	public void RemoveLines(){
		foreach (Transform child in transform) {
			if(child.gameObject.name.Contains("Line")){
				ObjectPool.RemoveLine(child.gameObject);
			}
		}
	}

	public virtual void SetUpGrid(){
		
		DisplayToken.MAX_TYPE = 4;
		randomizeGrid (true);
		
		while (hasMatch(false)) {	
			randomizeGrid (false);
		}
	}

	// Update is called once per frame
	void Update ()
	{	
//		Debug.Log (DisplayToken.MAX_MATCH);

		if(!isSet){
			SetUpGrid();
			isSet = true;
		}

		GameObject mover = gameObject;

		if(transform.parent != null){
			mover = transform.parent.gameObject;
		}

		if(mover.transform.localScale.x < 1){
			RemoveLines();
		}

		if(mover.GetComponent<LerpMove>().inPos && !hasLines){
			SetUpLines();
			hasLines = true;
		}

		if(!inAnim()){
			if(!hasMatch(true)){
				if(isFull()){
					GameOver();
//					Debug.Log ("FULL!  Game over!");
//					Application.LoadLevel("EndScreen");
				}
			} else {
				foreach(GridRockToken grt in rocks){
					grt.hasMatch(true);
				}
			}
		}
	}
	
	public static bool inAnim(){
		
//		Debug.Log ("TokenMovement.inAnim:" + TokenMovement.inAnim);
//		Debug.Log ("DisplayToken.inAnim:" + DisplayToken.inAnim);

		bool result = TokenMovement.inAnim || DisplayToken.inAnim;
		
		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 0; y < GRID_HEIGHT; y++) {
				if(grid [x, y] != null){
					result = result || grid [x, y].marked;
				}
			}
		}

		return result;
	}
	
	public void randomizeGrid (bool create)
	{
		
		Debug.Log("randomizeGrid");
		
		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 7; y < GRID_HEIGHT; y++) {

				if(create){

					GameObject newToken;

					newToken = ObjectPool.GetToken();

					grid [x, y] = newToken.GetComponent<GridToken> ();

					grid [x, y].SetGridPosAndPos(new PositionMessage(x, y));
					grid [x, y].GetComponent<TokenMovement>().moveToken(cols[x], rows[y]);
				} else {
					grid [x, y].gameObject.GetComponent<DisplayToken>().SetType(Random.Range(0, DisplayToken.MAX_TYPE));
				}
			}	
		}
	}

	public static void rowUp ()
	{

		//Debug.Log("rowUp");

		for (int x = 0; x < GRID_WIDTH; x++) {
			if (grid [x, 0] != null) {
				gameOver = true;
			}
		}

		if (gameOver) {
			GameOver();
		}


		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 0; y < GRID_HEIGHT; y++) {
				if (grid [x, y] != null) {
					grid [x, y].GetComponent<TokenMovement>().gridRestructNewDest(new PositionMessage(x, y - 1));
				}
			}
		}

		
		makeNewRow (true);
		while (hasMatch(false)) {
			makeNewRow (false);
		}
	}

	static void GameOver(){
		int highScore = PlayerPrefs.GetInt(GameManager.PREF_HIGH_SCORE);
		int highLevel = PlayerPrefs.GetInt(GameManager.PREF_HIGH_LEVEL);
		int totalGames = PlayerPrefs.GetInt(GameManager.PREF_GAMES_PLAYED);
		int totalScore = PlayerPrefs.GetInt(GameManager.PREF_TOTAL_SCORE);
		
		if(GameManager.score >= highScore){
			Debug.Log ("highScore: " + highScore);
			PlayerPrefs.SetInt(GameManager.PREF_HIGH_SCORE, GameManager.score);
			if(GameManager.score > 500){
				if(PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL) < StartScreen.MODE_CHALLENGE){ 
					PlayerPrefs.SetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_CHALLENGE);
				}
			}
		}
		if(level >= highLevel){
			PlayerPrefs.SetInt(GameManager.PREF_HIGH_LEVEL, level);
		}
		PlayerPrefs.SetInt(GameManager.PREF_GAMES_PLAYED, totalGames + 1);
		PlayerPrefs.SetInt(GameManager.PREF_TOTAL_SCORE, totalScore + GameManager.score);
		
		Debug.Log ("GameOver!!!");
		//			Application.LoadLevel("EndScreen");
		
		GameObject panel = GameObject.Find("Canvas");
		
		foreach (Transform child in panel.transform) {
			if(child.gameObject.name == "FailPanel"){
				child.gameObject.SetActive(true);
			}
		}
	}

//	static void printBottom(string pre){
//		for (int x = GRID_WIDTH - 1; x >= 0; x--) {
//			if(grid [x, GRID_HEIGHT - 1] == null)
//			{
//				Debug.Log(pre + "grid [" + x +", " + (GRID_HEIGHT - 1) + "]: ");
//			}
//		}
//	}
	
	public static void makeNewRow (bool makeNewRow)
	{
		for (int x = GRID_WIDTH - 1; x >= 0; x--) {
			if(makeNewRow){
				GameObject newToken;

				if(x != RowUpToken.rockIndex){
					newToken = ObjectPool.GetToken();
//					newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/NumberToken", typeof(GameObject)));
//					newToken.GetComponent<DisplayNumberToken>().setNumber(level/10f);
				} else {
					newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Rock", typeof(GameObject)));
				}

				newToken.gameObject.GetComponent<DisplayToken>().StartUp();

				int y = GRID_HEIGHT - 1;
				
				grid [x, y] = newToken.GetComponent<GridToken> ();
				
				grid [x, y].SetGridPosAndPos(new PositionMessage(x, y));
				grid [x, y].GetComponent<TokenMovement>().moveToken(cols[x], rows[y]);
			} else {
				GameObject newToken = grid [x, GRID_HEIGHT - 1].gameObject; //EXCEPTION
//			NullReferenceException: Object reference not set to an instance of an object
//					GridHandler.makeNewRow (Boolean makeNewRow) (at Assets/Scripts/grid/GridHandler.cs:311)
//						GridHandler.rowUp () (at Assets/Scripts/grid/GridHandler.cs:275)
//						RowUpToken.Step () (at Assets/Scripts/next/RowUpToken.cs:81)

				newToken.gameObject.GetComponent<DisplayToken>().StartUp();
			}
		}
	}

	public static void removeHoles ()
	{	
		for (int x = 0; x < GRID_WIDTH; x++) {
			//int holes = 0;
			for (int y = GRID_HEIGHT - 1; y > 0; y--) {
				if (grid [x, y] == null) {
					//holes = 1;
					for (int y2 = y - 1; y2 >= 0; y2--){
						if (grid [x, y2] != null) {
					 		grid [x, y2].SendMessage("gridRestructNewDest", new PositionMessage(x, y2 + 1), SendMessageOptions.RequireReceiver);
						}
					}
				}
			}
		}
	}
	
	public static bool hasMatch (bool markMatched)
	{	
		bool hasMatch = false;

		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 0; y < GRID_HEIGHT; y++) {
				GridToken token = grid [x, y];
				
				if (token != null) {
					hasMatch = token.hasMatch (markMatched) || hasMatch;
				}
			}
		}
	
		if (markMatched) {
			removeMatches ();
		}

		return hasMatch;
	}
	
	public static void removeMatches ()
	{

		int multis = 0;
		int bombs = 0;
		int zaps = 0;
		int nones = 0;

		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 0; y < GRID_HEIGHT; y++) {
				GridToken token = grid [x, y];
				if (token != null) {
					int dirs = 0;
					for (int dir = 0; dir < 4; dir++) {
						if (token.streak [dir] > 0) {
							dirs++;
						}
					}
					
					if (dirs > 1) {
						multis++;
					}
				}
			}
		}
		
		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 0; y < GRID_HEIGHT; y++) {
				GridToken token = grid [x, y];
				if (token != null) {
					
					for (int dir = 0; dir < 4; dir++) {
						if (token.streak [dir] > 0 && !token.marked) {
							int streak = token.streak [dir];
	
							switch (streak) {
							case 3:
								nones++;
								break;
							case 4:
								bombs++;
								break;
							case 5:
							case 6:
							case 7:
							case 8:
								zaps++;
								break;
							}
							
//							token.markToken();
//							token.streak [dir] = 0;
						}
					}
				}
			}
		}

		nones /= 3;
		bombs /= 4;
		zaps /= 5;
		nones -= multis;
		
		for (int n = 0; n < nones; n++) {
			GameManager.addPowerUp(GridToken.POWERS.POWER_NONE);
			GameManager.mult++;
		}
		for (int m = 0; m < multis; m++) {
			GameManager.addPowerUp (GridToken.POWERS.POWER_MULT_X1);
			GameManager.mult++;
		}
		for (int b = 0; b < bombs; b++) {
			GameManager.addPowerUp (GridToken.POWERS.POWER_NONE);
			GameManager.addPowerUp (GridToken.POWERS.POWER_BOMB);
			GameManager.mult += 2;
		}
		for (int z = 0; z < zaps; z++) {
			GameManager.addPowerUp (GridToken.POWERS.POWER_NONE);
			GameManager.addPowerUp (GridToken.POWERS.POWER_COLOR_ZAP);
			GameManager.mult += 3;
		}
		
		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 0; y < GRID_HEIGHT; y++) {
				GridToken token = grid [x, y];
				if (token != null) {
					
					for (int dir = 0; dir < 4; dir++) {
						if (token.streak [dir] > 0 && !token.marked) {
							token.markToken();
							token.streak [dir] = 0;
						}
					}
				}
			}
		}

		foreach(TextMesh points in pointDisplays){
			points.text = "+" + GameManager.mult;
		}

		pointDisplays.Clear();
	}

	public bool isEmpty(){
		bool empty = true;
		
		for (int x = 0; x < GRID_WIDTH; x++) {
			empty = empty && (grid [x, GRID_HEIGHT - 1] == null);
		}
		
		return empty;
	}
	
	public bool isFull ()
	{
		bool full = true;
		
		for (int x = 0; x < GRID_WIDTH; x++) {
			full = full && grid [x, 0] != null;
		}
		
		return full;
	}

//	public static int GetSweetToken (){
//		List<int> results = new List<int> ();
//
//		GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));
//
//		for (int x = 0; x < GRID_WIDTH; x++) {
//			for (int y = GRID_HEIGHT - 1; y > -1; y--) {
//				GridToken token = grid [x, y];
//				
//				if (token == null) {
//					for (int t = 0; t < DisplayToken.MAX_TYPE; t++) {
//						temp.GetComponent<DisplayToken>().SetType(t);
//						temp.GetComponent<GridToken>().SetGridPos(new PositionMessage(x, y));
//						if(hasMatch(false)){
//							results.Add (t);
//						}
//						grid[x, y] = null;
//					}
//					break;
//				}
//			}
//		}
//		
//		Destroy(temp);
//
//		if(results.Count > 0){
//			return (results[Random.Range(0, results.Count)]);
//		} else {
//			return -1;
//		}
//	}
}
