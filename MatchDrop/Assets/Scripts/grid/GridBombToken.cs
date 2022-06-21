using UnityEngine;
using System.Collections;

public class GridBombToken : GridToken {
	
	public override void markToken(){
		base.markToken();
		
		for(int x = -1; x <= 1; x++){
			for(int y = -1; y <= 1; y++){
				int markX = gridX + x;
				int markY = gridY + y;
				
				if(markX >= 0 && markX < GridHandler.GRID_WIDTH &&
				   markY >= 0 && markY < GridHandler.GRID_HEIGHT){

					if(grid[markX, markY] != null && !grid[markX, markY].marked){
						if(grid[markX, markY].GetComponent<GridRockToken>()){
							Debug.Log("ROCK");
						}

						if(grid[markX, markY].GetComponent<GridNumberToken>() != null){
							grid[markX, markY].GetComponent<GridNumberToken>().reduceToken(1);
						} else {
							grid[markX, markY].markToken();
						}
					}
				}
			}	
		}
	}
}
