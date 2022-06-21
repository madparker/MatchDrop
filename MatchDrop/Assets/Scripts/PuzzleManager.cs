using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PuzzleManager : GameManager {

	protected int CurrentLevel{
		set{
			levels[currentLevel].Deactivate();
			currentLevel = value;
			if(currentLevel < levels.Length){
				levels[currentLevel].Activate();
			}
		}

		get{
			return currentLevel;
		}
	}

	protected static int currentLevel;
	protected int highLevel;
	
	public static string PREF_PUZZLE_LEVEL = "puzzleLvl";
	public static string PREF_PUZZLE_HIGH = "highLvl";

	// Use this for initialization
	void Start () {
		unlockPrev = false;
	}
	
	public override void Setup(){

		levels = pLevels;

		currentLevel = PlayerPrefs.GetInt(PREF_PUZZLE_LEVEL, 0);
		highLevel = PlayerPrefs.GetInt(PREF_PUZZLE_HIGH, 0);

		Debug.Log("currentLevel: " + currentLevel);
		
		SetUpGridAndNext();

		PuzzleLevelButton.puzzleLevelOffset = (currentLevel/9) * 9;

		Destroy (Next);
	}

	public void SetUpGridAndNext(){
		
		nextTokenHandler = (GameObject)Instantiate(Resources.Load("Prefabs/ScreenGame/NextTokenOrderedHandler", typeof(GameObject)));
		
		gridHandler = GameObject.Find("GridPuzzleHandler");

		updateLevel(true);

		nextTokenHandler.GetComponent<NextTokenHandler>().Setup();
		nextTokenHandler.transform.parent = this.transform;
		
		GridToken.gridHandler = gridHandler.GetComponent<GridHandler>();
		
		GridToken.nextHandler = nextTokenHandler.GetComponent<NextTokenHandler>();
	}

	public void updateLevel(){
		updateLevel (false);
	}

	public virtual bool updateLevel(bool init){

		if(GetComponent<TutorialManager>() == null){
			PlayerPrefs.SetInt(PREF_PUZZLE_LEVEL, currentLevel);
		}

		if(currentLevel > highLevel){
			PlayerPrefs.SetInt(PREF_PUZZLE_HIGH, currentLevel);
			highLevel = currentLevel;
		}

		if(currentLevel >= 10){
			if(PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL) < StartScreen.MODE_ENDLESS){ 
				PlayerPrefs.SetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_ENDLESS);
			}
		}

		return levelUp(init);
	}

	public bool levelUp(bool init){
		
		if(currentLevel >= levels.Length){
			return false;
		}
		
		Level level = levels[currentLevel];
		nextTokenHandler.GetComponent<NextTokenOrderedHandler>().SetOrder(level.nextInts);
		
		gridHandler.GetComponent<GridPuzzleHandler>().SetOrder(level.charGrid);
		
		if(!init){
			gridHandler.GetComponent<GridPuzzleHandler>().SetUpGrid();
		}
		getNextToken();
		
		GridHandler.level = currentLevel;
		score = 0;
		updateScoreLevelDisplay();
		
		return true;
	}

	
	public override void getNextToken(){

		if(nextTokenHandler.GetComponent<NextTokenHandler>().getCount() > 0){
			base.getNextToken();
		} else {

			if(!inAnim && !GridHandler.inAnim()  && !GridHandler.hasMatch(false)){
				if(gridHandler.GetComponent<GridHandler>().isEmpty()){
					Debug.Log(inAnim);
					currentLevel++;
					if(!updateLevel(false)){
						GoToEndScreen();
					}
				} else {
					GameManager.hasOverlay = false;
					GameObject panel = GameObject.Find("Canvas");

					foreach (Transform child in panel.transform) {
						if(child.gameObject.name == "FailPanel"){
							child.gameObject.SetActive(true);
						}
					}
				}
			}
		}
	}

	public virtual void GoToEndScreen(){
		Application.LoadLevel("EndScreen");
	}
	
	public override void updateScoreLevelDisplay(){
		score += mult;

//		Debug.Log("scoreTxt: " + scoreTxt.GetComponent<Text>().text);

		scoreTxt.GetComponent<Text>().text = "LEVEL: " + GridHandler.level;
	}

	public static Level[] pLevels = {
		new Level(
			"2",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"........",		//6
			"22......"		//7
		}
		), 
		new Level(
			"3",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"...3....",		//6
			"...3...."		//7
		}
		), 
		new Level(
			"11",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"........",		//6
			"11......"		//7
		}
		),
		new Level(
			"341",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"...34...",		//6
			".1314..."		//7
		}
		),
		new Level(
			"04",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"..4.....",		//5
			".4211...",		//6
			"22100..."		//7
		}
		),  
		new Level(
			"213",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"...2....",		//5
			"..11....",		//6
			"..323..."		//7
		}
		), 
		new Level(
			"020",
			new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"...1....",		//4
			"...1....",		//5
			"..20....",		//6
			"..21...."		//7
		}
		), 
		new Level(
			"44",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"........",		//6
			"44..4..."		//7
		}
		), 
		new Level(
			"221",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"..11....",		//6
			".121...."		//7
		}
		), 
		new Level(
			"420",
		new string[]{
			"........",		//0
			"........",		//1
			"...2....",		//2
			"...1....",		//3
			"...3....",		//4
			"..04....",		//5
			"..43....",		//6
			"0.1312.."		//7
		}
		), 
		new Level(
			"220",
		new string[]{
			"........",		//0
			"........",		//1
			"..123...",		//2
			"..102...",		//3
			"..430...",		//4
			"..344...",		//5
			"..10011.",		//6
			"..10122."		//7
		}
		), 
		new Level(
			"1010101",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"........",		//6
			".......0"		//7
		}
		),
		new Level(
			"31032",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"..00....",		//5
			"..13....",		//6
			"..122..."		//7
		}
		),
		new Level(
			"2032",
			new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			".22.....",		//4
			".00.....",		//5
			"044....2",		//6
			"033422.2"		//7
		}
		), 
		new Level(
			"310023",
		new string[]{
			"........",		//0
			"...1....",		//1
			"..30....",		//2
			"..20....",		//3
			"..32.4..",		//4
			"..30.4..",		//5
			"..11320.",		//6
			"..11342."		//7
		}
		),  
		new Level(
			"313121",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"...22...",		//6
			"112321.."		//7
		}
		), 
		new Level(
			"33212103",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			"........",		//5
			"........",		//6
			"..2100.."		//7
		}
		), 
		new Level(
			"023240",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"........",		//4
			".4.44...",		//5
			".4432...",		//6
			"40432..."		//7
		}
		), 
		new Level(
			"1312211",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"....0...",		//3
			"...03...",		//4
			"...31...",		//5
			"...00.1.",		//6
			".110021."		//7
		}
		),
		new Level(
			"1420030",
		new string[]{
			"1.......",		//0
			"23......",		//1
			"21......",		//2
			"01.2....",		//3
			"2002....",		//4
			"3140....",		//5
			"1242....",		//6
			"1211...."		//7
		}
		),
		new Level(
			"444313",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"..2.....",		//3
			"1303....",		//4
			"2311....",		//5
			"12044...",		//6
			"1302244."		//7
		}
		),
		new Level(
			"0243210",
		new string[]{
			"........",		//0
			".....0..",		//1
			".....4..",		//2
			"....40..",		//3
			"....30..",		//4
			"....23..",		//5
			"...012..",		//6
			"...221.."		//7
		}
		),
		new Level(
			"33412",
		new string[]{
			"........",		//0
			"........",		//1
			"....0...",		//2
			"....01..",		//3
			"....13..",		//4
			"....04..",		//5
			"...214..",		//6
			"...201.."		//7
		}
		),
		new Level(
			"340200",
		new string[]{
			"........",		//0
			"........",		//1
			"........",		//2
			"........",		//3
			"......1.",		//4
			"......1.",		//5
			"..2.113.",		//6
			".4420013"		//7
		}
		),
		new Level(
			"00031",
			new string[]{
			".....312",		//0
			"..0..012",		//1
			"..2..300",		//2
			"..1..312",		//3
			"312..012",		//4
			"30200300",		//5
			"20144312",		//6
			"02211412"		//7
		}
		),
		new Level(
			"1",
			new string[]{
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"...214..",		
			"...4q2..",		
			"...203..",		
		}
		),
		new Level(
			"a",
			new string[]{
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			".11.22..",		
		}
		),
		new Level(
			"as",
			new string[]{
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			".11.33..",		
		}
		),
		new Level(
			"1*",
			new string[]{
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"1.1.1.1.",		
		}
		),
		new Level(
			"34*",
			new string[]{
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"4.3.d.34",		
		}
		),
		new Level(
			"w*",
			new string[]{
			"........",		
			"........",		
			"........",		
			"........",		
			"........",		
			"41414141",		
			"32332332",			
			"41414141",	
		}
		),
		new Level(
			"3*",
			new string[]{
			"........",		
			"...1.1..",		
			"...3.3..",		
			"...1.1..",		
			"...3.3..",		
			"...1.1..",		
			"...3.3..",		
			"...1.1..",	
		}
		),
		new Level(
			"e",
			new string[]{
			"........",	
			"........",	
			"........",
			"........",	
			"........",	
			"..4.4...",	
			"..3.3...",	
			"..121...",		
		}
		)
	};
	
	public static Level[] levels = new Level[]{};
	public static int totalLevels = levels.Length;
}
