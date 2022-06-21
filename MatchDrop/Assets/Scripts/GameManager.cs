using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{	
	private static GameObject next;

	public static GameObject Next
	{
		get
		{
			return next;
		}

		set
		{
			next = value;
		}
	}

	public static GameObject nextTokenHandler;
	public static GameObject gridHandler;
	public static GameObject scoreTxt;

	public static int mult;
	public static int score;

	public static int dropCol;

	public static bool isHighScore = false;
	public static bool isHighLevel = false;
	public static bool fallingTokens = false;
	public static bool inAnim = false;
	
	public GameObject highLight;

	public static bool touchDown;
	
	public static string PREF_TOTAL_SCORE = "prefScore";
	public static string PREF_GAMES_PLAYED = "gamesPlayed";
	public static string PREF_HIGH_SCORE = "highScore";
	public static string PREF_HIGH_LEVEL = "highLevel";
	public static string PREF_LOCK_LEVEL = "locked";
	
	public delegate void DisplayDelegate();
	public static DisplayDelegate updateScoreLevelDelegate;
	
	public static int[] tokensRemoved;
	public static int[] powerUpsRemoved;

	public static bool unlockPrev = true;

	public static bool hasOverlay = false;

	public static bool touchDevice = false;

	public static float GUI_Y;

	public static GameObject gameManager;
	
	public AudioClip matchSound;
	public AudioClip levelUpSound;
	public AudioClip dropSound;
	
	public static GameObject getGameManager(){
		return gameManager;
	}

	// Use this for initialization
	void Awake ()
	{
		DisplayToken.isInit = false;

		gameManager = gameObject;

		unlockPrev = true;

		GridHandler.Init();
		
		score = 0;
		
		isHighScore = false;
		isHighLevel = false;
		
//		scoreTxt = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/TextHolder", typeof(GameObject)));
		scoreTxt = GameObject.Find("ScoreLevel");
//		scoreTxt.transform.position = new Vector3(0, 4.25f, -1.1f);

//		scoreTxt.GetComponent<TextMesh>().fontSize = (int)20;

//		scoreTxt.transform.parent = transform;

		foreach (Transform child in transform){
			if(child.gameObject.name == "HighLight"){
				highLight = child.gameObject;
			}
		}

		updateScoreLevelDelegate = new DisplayDelegate(updateScoreLevelDisplay);
		
		updateScoreLevelDelegate();

		tokensRemoved = new int[]{0, 0, 0, 0, 0, 0, 0};
		powerUpsRemoved = new int[]{0,0,0,0}; //zap, blank, bomb, multi

		Setup();

		
		GUI_Y = GridHandler.rows[0] - GridHandler.GRID_SIZE * 2;


//		GameObject save = GameObject.Find("SaveToken");
//		if(save != null){
//			GameObject sButton = GameObject.Find("SaveButton");
//			save.transform.parent = sButton.transform;
//
//			//FIX ME
//		}
	}

	public virtual void Setup(){
		hasOverlay = false;

		nextTokenHandler = (GameObject)Instantiate(Resources.Load("Prefabs/ScreenGame/NextTokenHandler", typeof(GameObject)));
		gridHandler = (GameObject)Instantiate(Resources.Load("Prefabs/ScreenGame/GridHandler", typeof(GameObject)));
		
		nextTokenHandler.GetComponent<NextTokenHandler>().Setup();
		
		NextTokenHandler.bag.Clear ();

//		next = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));//(GameObject)Instantiate(Resources.Load("new/Token", typeof(GameObject)));
//		next.transform.parent = this.transform;
//		positionNextToken ();
		
		nextTokenHandler.transform.parent = this.transform;
		gridHandler.transform.parent = this.transform;
		
		GridToken.nextHandler = nextTokenHandler.GetComponent<NextTokenHandler>();
				
//		next.transform.localScale = new Vector3 (DisplayToken.startSize, DisplayToken.startSize, DisplayToken.startSize);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		#if UNITY_ANDROID
		touchDevice = true;
		#endif
		
		#if UNITY_IOS
		touchDevice = true;
		#endif
		
		#if UNITY_EDITOR
		touchDevice = false;
		#endif

		if (HintHelper.currentHint == null && !GridHandler.inAnim () && GridHandler.gameOver) {
			hasOverlay = true;
		}

		if (!GameManager.hasOverlay) {

			InputMoveToken();
			InputDropToken();

			TokenMovement.updateTokens();
		}
	}

	public virtual void InputDropToken(){
	
		if (touchDevice) {
			if (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Ended) &&
			    Camera.main.ScreenToWorldPoint(Input.GetTouch (0).position).x > GridHandler.cols [0] - DisplayToken.startSize * .75f &&
			    Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position).y < GUI_Y  && next != null) {
				dropToken ();
			}
		} else {
			if (Input.GetMouseButtonDown (0) && 
			    Camera.main.ScreenToWorldPoint(Input.mousePosition).x > GridHandler.cols [0] - DisplayToken.startSize * .75f &&
			    Camera.main.ScreenToWorldPoint (Input.mousePosition).y < GridHandler.rows[GridHandler.rows.Length - 1] && next != null) {
				dropToken ();
			}
		}
	}

	public void InputMoveToken(){
		
		if (next == null && GetComponent<LerpMove>().inPos && HintHelper.currentHint == null) {
			inAnim = false;
			getNextToken ();
		}

		Vector3 pos;

		if (touchDevice && Input.touchCount > 0) {
			pos = Camera.main.ScreenToWorldPoint(Input.GetTouch (0).position);
		} else {
			pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		
		if (next != null && next.GetComponent<RowUpToken> () == null && !GridHandler.inAnim () &&
		    GetComponent<LerpMove>().inPos && 
		    pos.y < GridHandler.rows[GridHandler.rows.Length - 1] && 
		    pos.y > GUI_Y ) {
			inAnim = false;
			positionNextToken();
			highLight.gameObject.GetComponent<MeshRenderer> ().enabled = true;
		} else if(next != null && next.GetComponent<RowUpToken> () == null && !GridHandler.inAnim () &&
		          GetComponent<LerpMove>().inPos){
			highLight.gameObject.GetComponent<MeshRenderer> ().enabled = true;
		} else {
			inAnim = true;
			highLight.gameObject.GetComponent<MeshRenderer> ().enabled = false;
		}
	}

	public void positionNextToken ()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (touchDevice) {
			if(Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Moved){
				
				highLight.transform.position = new Vector3(
					next.transform.position.x, 
					highLight.transform.position.y,
					highLight.transform.position.z);
				return;
			}
		}

		Vector3 pos = next.transform.position;
		
		float posY = GridHandler.rows[0] - GridHandler.GRID_SIZE;
		
		if (mousePos.x < GridHandler.cols [0]) {
			next.transform.position = new Vector3 (GridHandler.cols [0], posY, pos.z);
			dropCol = 0; 
		} else {
			int i = 0;
			foreach (float posX in GridHandler.cols) {
				if (mousePos.x > posX - GridHandler.GRID_SIZE/2) {
					next.transform.position = new Vector3 (posX, posY, pos.z);
					dropCol = i;
				}
				i++;
			}
		}
		
		next.GetComponent<TokenMovement>().moveToken(next.transform.position.x, next.transform.position.y);

		highLight.transform.position = new Vector3(
			next.transform.position.x, 
			highLight.transform.position.y,
			highLight.transform.position.z);
	}
	
	public virtual void dropToken ()
	{
		
		if (!GridHandler.inAnim()){

			highLight.gameObject.GetComponent<MeshRenderer>().enabled = false;
	
			TokenMovement move = next.GetComponent<TokenMovement>();
			
			if (move.atDestination ()) {
				GetComponent<AudioSource>().pitch = 0.8f;
				
				mult = 0;
			
				for (int x = 0; x < GridHandler.GRID_WIDTH && next != null; x++) {
					if (next.transform.position.x >= GridHandler.rows [x] && 
					    next.transform.position.x < GridHandler.rows [x] + GridHandler.GRID_SIZE) {

						for (int y = GridHandler.GRID_HEIGHT - 1; y >= 0; y--) {
							if (GridHandler.grid [x, y] == null) {
							
								next.GetComponent<TokenMovement>().setNewDestWithGridPos(new PositionMessage(x, y));
					
								GridHandler.next = next.GetComponent<TokenMovement>();
								
								next.transform.parent = gridHandler.transform;
								next = null;

								break;
							}
						}
						Invoke ("playDropSound", 0.05f);
					}
				}
			}
		}
	}
	
	public void playMatchSound(){
		GetComponent<AudioSource>().pitch += 0.05f;
		GetComponent<AudioSource>().PlayOneShot(matchSound);
	}
	
	public void playRowUpSound(){
		GetComponent<AudioSource>().pitch += 0.05f;
		GetComponent<AudioSource>().PlayOneShot(levelUpSound);
	}
	
	public void playDropSound(){
		GetComponent<AudioSource>().pitch += 0.05f;
		GetComponent<AudioSource>().PlayOneShot(dropSound);
	}

	public virtual void getNextToken(){
		next = nextTokenHandler.GetComponent<NextTokenHandler>().getNextToken().gameObject;
	}
	
	public static void addPowerUp (GridToken.POWERS power)
	{
		NextTokenHandler.powerUps.Add(power);
	}
	
	public virtual void updateScoreLevelDisplay(){
		score += mult;
		
		scoreTxt.GetComponent<Text>().text = "SCORE: " + score + "   LEVEL: " + GridHandler.level;
	}

	public void MainScreen(){
		Application.LoadLevel("StartScreen");
	}
}

