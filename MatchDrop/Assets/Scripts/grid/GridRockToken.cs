using UnityEngine;
using System.Collections;

public class GridRockToken : GridToken {

	void Start () {
		GridHandler.rocks.Add (this);
	}


	public override bool hasMatch(bool markMatched){
		if(!marked){
			for (int x = -1; x < 2; x++) {
				for (int y = -1; y < 2; y++) {
					
					int indexX = gridX + x;
					int indexY = gridY + y;
					
					if(indexX > -1 && indexX < GridHandler.GRID_WIDTH &&
					   indexY > -1 && indexY < GridHandler.GRID_HEIGHT){
						
						GridToken other = grid[indexX, indexY];

						if(other != null && other.marked){

							if(markMatched){
								markToken();
							}

							return true;
						}
					}
				}
			}
		}
		
		return false;
	}

	public override bool hasDirMatch(DIRS dir, int matchType, bool markMatched){
		return false;

	}

	public override bool isMatch (int otherType)
	{	
		return false;
	}

//	public override void markToken ()
//	{
//		base.markToken ();
//		
//		GridToken gridToken = gameObject.GetComponent<GridToken>();
//
//		gridToken.remove();
//		if(gameObject != null){
//			Destroy (gameObject);
//		}
//	}
}
