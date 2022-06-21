using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChallengeManager : GameManager
{

		public static string LABEL_MOVES_LEFT = "Moves:\n";
		public static string PREF_CHALLENGE_LEVEL = "chalLvl";
		public static string PREF_CHALLENGE_HIGH = "chalHighLvl";
		public GameObject dropNumberText;
		public int totalScore;
		public int totalMatches;
		public bool hasMatchLevel = false;
		public int[] matchLevels;
		public bool hasPowerLevel = false;
		public int[] powerLevels;
		public int totalTokensRemoved;
		private static bool init = false;
		private int currentWinCon = 0;
		private int highLevel = 0;
		private static string[] powerUpNames = {
		"Zap",
		"Blank",
		"Bomb",
		"Mulit"
	};
		public static int POWERUP_ZAP = 0;
		public static int POWERUP_BOMB = 1;
		public static int POWERUP_MULTI = 2;
		public static int POWERUP_BLANK = 3;
		bool isInit = true;
		public static List<GameObject> challengeDisplays;

		//RESTRICTIONS
		public static int moves = -1;
	
		public override void Setup ()
		{
				currentWinCon = PlayerPrefs.GetInt (PREF_CHALLENGE_LEVEL, 0);
				highLevel = PlayerPrefs.GetInt (PREF_CHALLENGE_HIGH, 0);

				if (currentWinCon > highLevel) {
						PlayerPrefs.SetInt (PREF_CHALLENGE_HIGH, currentWinCon);
						highLevel = currentWinCon;
						Debug.Log ("HIGH: " + highLevel);
				}

				foreach (Transform child in transform) {
						if (child.gameObject.GetComponent<GridHandler> () != null) {
								Destroy (child.gameObject);
						}
						if (child.gameObject.GetComponent<NextTokenHandler> () != null) {
								Destroy (child.gameObject);
						}
				}

//				Destroy (next);

				if (challengeDisplays != null) {
						foreach (GameObject gameObject in challengeDisplays) {
								Destroy (gameObject);
						}
				}
		
				base.Setup ();
		
				score = 0;
				hasMatchLevel = false;
				hasPowerLevel = false;

				tokensRemoved = new int[7]{0, 0, 0, 0, 0, 0, 0};
				powerUpsRemoved = new int[]{0,0,0,0}; 
	
				challengeDisplays = new List<GameObject> ();

				int[] challenge = challenges [currentWinCon];
		
				int index = 0;
				totalScore = challenge [index++];
				totalMatches = challenge [index++];

				if (totalMatches > 0) {
						hasMatchLevel = true;
				}

				matchLevels = new int[7];
				int indexOffset = 0 - index;
				for(; index < matchLevels.Length - indexOffset; index++){
					matchLevels [index + indexOffset] = challenge [index];
				}

				powerLevels = new int[4];
				indexOffset = 0 - index;
				for(; index < powerLevels.Length - indexOffset; index++){
					powerLevels [index + indexOffset] = challenge [index];
				}

				moves = challenge [index++];

				int numMatches = 0;

				print("numMatches: " + numMatches + " index: " +index);

				for (int i = 0; i < matchLevels.Length; i++) {
						if (matchLevels [i] > 0) {
								hasMatchLevel = true;
								numMatches++;
								GameObject disp = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/TokenChallengeDisplay", typeof(GameObject)));
								disp.GetComponent<DisplayToken> ().SetType (i);
								disp.GetComponent<ChallengeTokenDisplay> ().textMesh.text = "0/" + matchLevels [i];
								disp.transform.parent = transform;
								challengeDisplays.Add (disp);
						}
				}

				for (int i = 0; i < powerLevels.Length; i++) {
						if (powerLevels [i] > 0) {
								hasPowerLevel = true;
								numMatches++;
				GameObject disp = new GameObject ();
								switch (i) {
								case 0:
										disp = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/ZapChallengeDisplay", typeof(GameObject)));
										break;
								case 1:
										disp = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/BombChallengeDisplay", typeof(GameObject)));
										break;
								case 2:
										disp = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/MultiChallengeDisplay", typeof(GameObject)));
										break;
								}

								disp.GetComponent<ChallengeTokenDisplay> ().textMesh.text = "0/" + powerLevels [i];
								disp.transform.parent = transform;
								challengeDisplays.Add (disp);


						}
				}

				float dispChalWidth = GridHandler.fieldRect.width / numMatches;
				
				for (int i = 0; i < numMatches; i++) {
						print("I: " + i);
						print("challengeDisplays: " + challengeDisplays.Count);
						print("numMatches: " + numMatches);
						challengeDisplays [i].transform.position = new Vector3 (
								dispChalWidth * i - GridHandler.fieldRect.width / 3,
								challengeDisplays [i].transform.position.y,
								0);
				}

				if (dropNumberText == null) {
					dropNumberText = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/TextHolder", typeof(GameObject)));
					dropNumberText.transform.position = new Vector3 (-2.7f, 3.25f, -1.1f);
			
					dropNumberText.GetComponent<TextMesh> ().fontSize = (int)20;

					dropNumberText.transform.parent = transform;
				}


				if(moves <= 0){
					moves = -1;
					dropNumberText.GetComponent<TextMesh> ().text = "";
				} else {
					dropNumberText.GetComponent<TextMesh> ().text = LABEL_MOVES_LEFT + moves;	
				}



				mult = 0;
				updateScoreLevelDisplay ();

				isInit = false;

		PuzzleLevelButton.puzzleLevelOffset = (currentWinCon/9) * 9;
	}

		public override void updateScoreLevelDisplay ()
		{
		
				base.updateScoreLevelDisplay ();
		
				string displayStr = "";
		
				if (totalScore > 0) {
						displayStr = "Total Score: " + score + "/" + totalScore + "\n";
				}

				totalTokensRemoved = 0;

				if (tokensRemoved != null) {
						for (int i = 0; i < tokensRemoved.Length; i++) {
								totalTokensRemoved += tokensRemoved [i];
						}
				}

				if (totalMatches > 0) {
						displayStr += "Tokens Removed: " + totalTokensRemoved + "/" + totalMatches + "\n";
				}

				scoreTxt.GetComponent<Text> ().text = displayStr;

				int displayIndex = 0;

				if (hasMatchLevel) {
						for (int i = 0; i < matchLevels.Length; i++) {
								if (matchLevels [i] > 0) {
										int displayNum = challengeDisplays [displayIndex].GetComponent<DisplayToken> ().type;
				
										challengeDisplays [displayIndex].GetComponent<ChallengeTokenDisplay> ().textMesh.text = 
						GameManager.tokensRemoved [displayNum] + "/" + matchLevels [i];
										displayIndex++;
								}
						}
				}
		
				if (hasPowerLevel) {
						int displayNum = 0;
						for (int i = 0; i < powerLevels.Length; i++) {
								if (powerLevels [i] > 0) {
										challengeDisplays [displayIndex].GetComponent<ChallengeTokenDisplay> ().textMesh.text = 
						GameManager.powerUpsRemoved [i] + "/" + powerLevels [i];
										displayIndex++;
										displayNum++;
								}
						}
				}

				Debug.Log ("currentWinCon: " + currentWinCon);

				if (!isInit && isWin ()) {
						currentWinCon++;
			
						PlayerPrefs.SetInt (PREF_CHALLENGE_LEVEL, currentWinCon);
						Debug.Log ("WIN!!!");
						Setup ();
				}
		}

		public bool isLoss(){
			bool loss = false;

			if(moves == 0){
				loss = true;
			} 

			return loss;
		}

		public bool isWin ()
		{
				bool isWin = true;
		
				if (score < totalScore) {
						isWin = false;
				}
		
				totalTokensRemoved = 0;

				if (hasMatchLevel) {
						for (int i = 0; i < DisplayToken.sprites.Length; i++) {
								print(GameManager.tokensRemoved.Length + "," + matchLevels.Length + 
										"=" + i);
								if (GameManager.tokensRemoved [i] < matchLevels [i]) {
										isWin = false;
								}
				
								totalTokensRemoved += GameManager.tokensRemoved [i];
						}
				}

				if (hasPowerLevel) {
						for (int i = 0; i < powerLevels.Length; i++) {
								if (GameManager.powerUpsRemoved [i] < powerLevels [i]) {
										isWin = false;
								}
						}
				}
		
				if (totalTokensRemoved < totalMatches) {
						isWin = false;
				}
		
				return isWin;
		}

		public override void dropToken ()
		{
				base.dropToken ();
				if (moves > 0) {
						moves--;
						dropNumberText.GetComponent<TextMesh> ().text = LABEL_MOVES_LEFT + moves;
				}

				if(isLoss()){
					Application.LoadLevel("EndScreen");
				}

		}

		//	Total Score, Total Tokens
		//	Match Type 1 - 7
		//	ColorZap, BombToken, MultiToken, BlankToken
		//  Number of Drops

		public static int[][] challenges = new int[][]{
		new int[]{9,0,
			0,0,0,0,0,0,0,
			0,0,0,0,
			20},
		new int[]{0,30,
			0,0,0,0,0,0,0,
			0,0,0,0,
			30},
		new int[]{0,0,
			6,0,6,6,0,0,0,
			0,0,0,0,
			28},
		new int[]{75,0,
			0,0,0,0,0,0,0,
			0,0,0,0,
			50},
		new int[]{0,75,
			0,0,0,0,0,0,0,
			0,0,0,0,
			50},
		new int[]{50,0,
			6,6,6,0,0,0,0,
			0,0,0,0,
			30},
		new int[]{150,100,
			0,0,0,0,0,0,0,
			0,0,0,0,
			75},
		new int[]{0,0,
			0,0,0,0,0,0,0,
			0,0,3,0,
			0},
		new int[]{0,0,
			0,0,0,0,0,0,0,
			0,3,0,0,
			0},
		new int[]{0,0,
			0,0,0,0,0,0,0,
			1,0,0,0,
			0},
		new int[]{100,0,
			10,10,10,10,10,0,0,
			0,0,0,0,
			0},
		new int[]{0,500,
			0,0,0,0,0,0,0,
			2,3,3,0,
			0},
		new int[]{1000,0,
			10,10,10,10,10,0,0,
			0,0,0,0,0,
			0}
	};
}
