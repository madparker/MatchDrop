using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridZapToken : GridToken {
	
	public override bool hasMatch(bool markMatched){
		GridToken below = null;
		
		if(gridY != GridHandler.GRID_HEIGHT -1){
			below =  grid[gridX, gridY + 1];
		}
		
		if(markMatched){
			GameManager.mult++;
		}
		
		if(below != null && !marked){
			if(below.GetComponent<GridBombToken>() != null){
				replaceTokensWithBombs(below);
			}
			
			List<GridToken> tokens = getAllTokens(below);
			
			//GridHandler.overlay.addLine(this, below, pApplet.color(0, 0, 255));
			
			GameManager.addPowerUp(GridToken.POWERS.POWER_NONE);
			
			foreach(GridToken token in tokens){
				//inAnim = inAnim || token.inAnim;
				if(!token.marked && markMatched){

					if(token.GetComponent<GridNumberToken>() != null){
						token.GetComponent<GridNumberToken>().reduceToken(1);
					} else {
						token.markToken();
					}
					//GridHandler.overlay.addLine(below, token, pApplet.color(0, 0, 255));
					//GridHandler.overlay.addPulse(this, token);

					GameObject line = ObjectPool.GetLine();//  (GameObject)Instantiate(Resources.Load("Prefabs/GUI/Line", typeof(GameObject)));
//					line.transform.parent = transform;
					line.GetComponent<Line>().SetUp(transform.position, token.transform.position, 10);
				}
			}
			
			GameManager.updateScoreLevelDelegate();
		}
		
		if(markMatched){
			markToken();
		}
		
		return true;
	}
	
	public void replaceTokensWithBombs(GridToken token){

		int matchType1 = token.GetComponent<DisplayToken>().type;
		int matchType2 = -1;
		
		if (token.GetComponent<GridMultiToken>() != null) {
			matchType2 = token.GetComponent<DisplayMultiToken>().type2;
		} 
		
		for (int x = 0; x < GridHandler.GRID_WIDTH; x++) {
			for (int y = 0; y < GridHandler.GRID_HEIGHT; y++) {
				if (grid[x, y] != null
				    && (grid[x, y].GetComponent<GridToken>().isMatch(matchType1) || 
				    grid[x, y].GetComponent<GridToken>().isMatch(matchType2))){
						GridToken oldToken = grid[x,y];
					GameObject newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
						newToken.GetComponent<DisplayToken>().type = oldToken.GetComponent<DisplayToken>().type;

						PositionMessage pm = new PositionMessage(oldToken.gridX, oldToken.gridY);

						newToken.GetComponent<GridToken>().SetGridPosAndPos(pm);
						ObjectPool.RemoveToken(oldToken.gameObject);
				}
			}
		}
	}
	
	public List<GridToken> getAllTokens(GridToken token) {
		
		List<GridToken> tokens = new List<GridToken>();
		
		int matchType1 = token.GetComponent<DisplayToken>().type;
		int matchType2 = -1;
		
		if (token.GetComponent<GridMultiToken>() != null) {
			matchType2 = token.GetComponent<DisplayMultiToken>().type2;
		} 
		
		for (int x = 0; x < GridHandler.GRID_WIDTH; x++) {
			for (int y = 0; y < GridHandler.GRID_HEIGHT; y++) {
				if (grid[x, y] != null
				    && (grid[x, y].GetComponent<GridToken>().isMatch(matchType1) || 
				    	grid[x, y].GetComponent<GridToken>().isMatch(matchType2))){
					tokens.Add(grid[x, y]);
				}
			}
		}

		return tokens;
	}
}
