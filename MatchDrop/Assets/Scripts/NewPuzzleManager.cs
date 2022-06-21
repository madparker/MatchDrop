using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class NewPuzzleManager : PuzzleManager
{
	
//	protected GameObject textobj;
	public Text hintText;

	public string endlessLevelName;
	public GameObject unlockPanel;
	public GameObject failPanel;
	public GameObject successPanel;

	public override void Setup ()
	{

//		textobj = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/TextHolder", typeof(GameObject)));
//		textobj.transform.position = new Vector3 (transform.position.x,
//		                                          transform.position.y,
//		                                          transform.position.z - 2);
//		textobj.transform.parent = transform;
//		
//		hintText = textobj.GetComponent<TextMesh> ();
//		hintText.fontSize = 20;
//		hintText.transform.position = new Vector3(0, 3.3f, 0);
		
		scoreTxt.transform.localScale = new Vector3(0, 0, -5);

		makeLevels();

		if(!hasOverlay)
			CurrentLevel = PlayerPrefs.GetInt(PuzzleManager.PREF_PUZZLE_LEVEL, 0);
		
		SetUpGridAndNext();
		
		GameManager.hasOverlay = false;

		Destroy (Next);
	}

	public static void makeLevels(){

		if(levels.Length == 0){
			TextAsset jsonData = Resources.Load("Files/puzzleLevels2") as TextAsset;

			string file = jsonData.text;
			
			JSONNode node = JSON.Parse(file);
			
			JSONArray jsonLevels = node["levels"].AsArray;
			
			PuzzleLevel[] pLevels = new PuzzleLevel[jsonLevels.Count];
			
			for(int i = 0; i < jsonLevels.Count; i++){
				pLevels[i] = new PuzzleLevel(jsonLevels[i]);
			}
			
			levels =  pLevels;
		}
	}

	public override void getNextToken(){		
		if(nextTokenHandler.GetComponent<NextTokenHandler>().getCount() > 0){
			base.getNextToken();
		} else {
			if(!inAnim && !GridHandler.inAnim()  && !GridHandler.hasMatch(false)){
				if(gridHandler.GetComponent<GridHandler>().isEmpty()){
//					if(!successPanel.activeSelf){
//						currentLevel++;
//					}
					if(!updateLevel(false)){
						GoToEndScreen();
					}
				} else {

					failPanel.SetActive(true);
					Text failText = failPanel.GetComponentInChildren<Text>();
					failText.text = ((PuzzleLevel)levels[CurrentLevel]).fail;
				}
			}
		}
	}

	public bool updateLevelOnButton(){

		if(CurrentLevel >= levels.Length){
//			if(PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL) < StartScreen.MODE_PUZZLE){ 
//				PlayerPrefs.SetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_PUZZLE);
//			}
			Application.LoadLevel("StartScreen");
			return true;
		}
		
		bool result = base.updateLevel (false);
		
		hintText.text = ((PuzzleLevel)levels[CurrentLevel]).hint;

		return result;
	}
	
	public override bool updateLevel(bool init){

		if(init){
//			CurrentLevel = 7;

			Debug.Log("CurrentLevel:" + CurrentLevel);

			hintText.text = ((PuzzleLevel)levels[CurrentLevel]).hint;
			return base.updateLevel(init);
		} else if(((PuzzleLevel)levels[CurrentLevel]).broadcast != null){
			BroadcastMessage(((PuzzleLevel)levels[CurrentLevel]).broadcast, true);
			return true;
		} else {
			inAnim = true;
			successPanel.SetActive(true);
			return true;
		}
	}

	public void nextPuzzle(){
		CurrentLevel++;
		retryPuzzle();
	}

	public void retryPuzzle(){
		Destroy (Next);
		failPanel.SetActive (false);
		successPanel.SetActive (false);
		unlockPanel.SetActive (false);
		GameManager.hasOverlay = false;
		updateLevelOnButton();
	}

	public void UnlockEndless(){
		PlayerPrefs.SetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_ENDLESS);
		unlockPanel.SetActive(true);
		unlockPanel.GetComponentInChildren<Text>().text = "Congrats!!!\nYou Unlocked Endless Mode!!!\nTry it now, or play more puzzles!";
	}
}
