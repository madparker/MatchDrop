using UnityEngine;
using System.Collections;

public class GridPuzzleHandler : GridHandler {
	
	char[,] tokenGrid;
	
	public void SetOrder(char[,] tokenGrid){
		this.tokenGrid = tokenGrid;
	}

	public override void SetUpGrid(){
		
		for (int x = 0; x < GRID_WIDTH; x++) {
			for (int y = 0; y < GRID_HEIGHT; y++) {
				if(grid [x, y] != null){
					Destroy(grid[x, y].gameObject);
				}

				GameObject newToken = charToToken(tokenGrid[x,y]);					

				if(newToken != null){
					grid [x, y] = newToken.GetComponent<GridToken> ();
					grid [x, y].SetGridPosAndPos(new PositionMessage(x, y));
				}
			}
		}
	}

	public static GameObject charToToken(char c){
		
		GameObject newToken = null;	

		switch(c){
		case 'p':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(0);
			break;
		case 'q':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(1);
			break;
		case 'w':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(2);
			break;
		case 'e':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(3);
			break;
		case 'r':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(4);
			break;
		case 'a':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/MultiToken", typeof(GameObject)));
			newToken.GetComponent<DisplayMultiToken>().SetTypes(1,2);
			break;
		case 's':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/MultiToken", typeof(GameObject)));
			newToken.GetComponent<DisplayMultiToken>().SetTypes(2,3);
			break;
		case 'd':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/MultiToken", typeof(GameObject)));
			newToken.GetComponent<DisplayMultiToken>().SetTypes(3,4);
			break;
		case 'f':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/MultiToken", typeof(GameObject)));
			newToken.GetComponent<DisplayMultiToken>().SetTypes(4,0);
			break;
		case '|':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Rock", typeof(GameObject)));
			break;
		case '*':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenZap", typeof(GameObject)));
			break;
		case '!':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/NumberToken", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(1);
			newToken.GetComponent<DisplayNumberToken>().UpdateGUI(1);
			break;
		case '@':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/NumberToken", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(2);
			newToken.GetComponent<DisplayNumberToken>().UpdateGUI(1);
			break;
		case '#':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/NumberToken", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(3);
			newToken.GetComponent<DisplayNumberToken>().UpdateGUI(1);
			break;
		case '$':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/NumberToken", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(4);
			newToken.GetComponent<DisplayNumberToken>().UpdateGUI(1);
			break;
		case ')':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/NumberToken", typeof(GameObject)));
			newToken.GetComponent<DisplayToken>().SetType(0);
			newToken.GetComponent<DisplayNumberToken>().UpdateGUI(1);
			break;
		case '=':
			newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/BlankToken", typeof(GameObject)));
			newToken.GetComponent<BlankTokenDisplay>().PreType = 3;
			break;
		default:
			int result = 0;
			if (int.TryParse(c + "", out result))
			{
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));
				newToken.GetComponent<DisplayToken>().SetType(result);
			}
			break;
		}

		return newToken;
	}
	
}
