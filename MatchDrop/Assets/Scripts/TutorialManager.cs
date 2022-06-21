using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class TutorialManager : PuzzleManager
{
	bool init = true;

	public override void Setup ()
	{
		scoreTxt.transform.localScale = new Vector3(0, 0, -5);

		Destroy(scoreTxt);

		if(!hasOverlay)
			CurrentLevel = 0;

		levels = tLevels;

		SetUpGridAndNext();

		GameManager.hasOverlay = false;

		Debug.Log ("Next: " + Next);

		Destroy (Next);
	}

	public override bool updateLevel(bool init){

		if(CurrentLevel >= tLevels.Length){
			if(PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL) < StartScreen.MODE_PUZZLE){ 
				PlayerPrefs.SetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_PUZZLE);
			}
			Application.LoadLevel("StartScreen");
			return true;
		}

		bool result = base.updateLevel (init);

		tLevels[CurrentLevel].StepLevel(this);

		return result;
	}

	public override void getNextToken(){

		if(init){
			init = false;
			base.getNextToken();
			tLevels[CurrentLevel].StepLevel(this);
		} else if(CurrentLevel >= tLevels.Length){
			if(PlayerPrefs.GetInt(GameManager.PREF_LOCK_LEVEL) < StartScreen.MODE_PUZZLE){ 
				PlayerPrefs.SetInt(GameManager.PREF_LOCK_LEVEL, StartScreen.MODE_PUZZLE);
				Application.LoadLevel("StartScreen");
			}
		} else {
			if(tLevels[CurrentLevel].step()){
				if(tLevels[CurrentLevel].levelComplete()){
					init = true;
					CurrentLevel++;
					updateLevel(false);
				} else {
					base.getNextToken();
					tLevels[CurrentLevel].StepLevel(this);
				}
			}
		}
	}

	public void setText(string text){
//		textMesh.text = text;
	}
	
	public override void updateScoreLevelDisplay(){
		score += mult;
		
		scoreTxt.GetComponent<TextMesh>().text = "";
	}
	
	public virtual void showLevelScore(){
		
		scoreTxt.GetComponent<TextMesh>().text = "LEVEL: " + GridHandler.level;
	}

	public TutorialStep.StepDelegate GetCondition(string condStr){

		MethodInfo method;

		if(condStr.Equals("empty")){
			method = GetType().GetMethod("Empty", 
				           BindingFlags.Public 
				           | BindingFlags.Static 
				           | BindingFlags.FlattenHierarchy);
		} else {
			method = GetType().GetMethod("Drop", 
				           BindingFlags.Public 
				           | BindingFlags.Static 
				           | BindingFlags.FlattenHierarchy);
		}
		
		return (TutorialStep.StepDelegate) Delegate.CreateDelegate
			(typeof(TutorialStep.StepDelegate), method);
	}

	public static bool dropped = false;

	public override void dropToken (){
		dropped = true;
		base.dropToken();
	}
	
	public static bool Drop()
	{
		if(dropped){
			dropped = false;
			return true;
		}

		return false;
	}
	
	public static bool Empty()
	{
		return gridHandler.GetComponent<GridHandler>().isEmpty();
	}
	
	public static bool falser()
	{
		return false;
	}

	public TutorialLevel[] tLevels;
//	= 
//		new TutorialLevel[]{
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Empty, "Drag your finger to move and drop tokens\n" + 
//			                 "to match 3 or more in a row."),
//		},
//		"2",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"........",		//5
//			"........",		//6
//			"...2.2.."		//7
//		}
//		),
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Drop, "You can match in any direction:\n" + 
//			                 "horizontally, vertically, or diagonally"),
//			new TutorialStep(Empty, "You can match in any direction:\n" + 
//			                 "horizontally, vertically, or diagonally")
//		},
//		"03",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"........",		//5
//			".3......",		//6
//			".3.00..."		//7
//		}
//		),
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Drop, "On the left you can see the next\n" + 
//			                 "tokens you will get to drop."),
//			new TutorialStep(Empty, "The token on top will be\n" +
//			                 "the next token for you to drop.")
//		},
//		"21",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"........",		//5
//			"...11...",		//6
//			"...22..."		//7
//		}
//		),
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Drop, "Remember, you match diagonally!"),
//			new TutorialStep(Empty, "There are also special tokens. Matching a\n" +
//			                 "Bomb Token (like the one in the middle)\n"+
//			                 "will remove all the surrounding tokens.")
//		},
//		"42",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"...42...",		//5
//			"..4w2...",		//6
//			"..101..."		//7
//		}
//		),
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Empty, "MultiTokens match tokens of\n" + 
//			                 "both types that they show!"),
//		},
//		"a",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"........",		//5
//			"........",		//6
//			".11.22.."		//7
//		}
//		),
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Empty, "ZapTokens remove all tokens\n" + 
//			                 "that match the one you dropped it on!"),
//		},
//		"*3",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"........",		//5
//			"........",		//6
//			"00.30300"		//7
//		}
//		),
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Drop, "NumberTokens need to be matched\n" + 
//			                 "more than once to be removed."),
//			new TutorialStep(Empty, "Once a NumberToken is matched\n" + 
//			                 "the number of times it displays,\n" +
//			                 "it becomes a normal token.")
//		},
//		"444",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"........",		//5
//			"........",		//6
//			"...4$..."		//7
//		}
//		),
//		new TutorialLevel(
//			new TutorialStep[]{
//			new TutorialStep(Drop, "Congrats!\n" + 
//			                 "You've unlocked Puzzle Mode!\n" +
//			                 "Go to the Start Screen to right to try it out!")
//		},
//		"0",
//		new string[]{
//			"........",		//0
//			"........",		//1
//			"........",		//2
//			"........",		//3
//			"........",		//4
//			"........",		//5
//			"........",		//6
//			"........"		//7
//		}
//		)
//	};
}
