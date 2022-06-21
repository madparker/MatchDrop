using UnityEngine;
using System.Collections;

public class PositionMessage {
	
	public int gridX, gridY;
	public Vector3 vec;
	
	public PositionMessage(int x, int y){
		this.gridX = x;
		this.gridY = y;

		vec.x = GridHandler.cols[x];
		if (y >= 0) {
			vec.y = GridHandler.rows[y];		
		} else {
			vec.y = GridHandler.rows[0] - GridHandler.GRID_SIZE;
		}
		vec.z = 0;
	}
	
	public PositionMessage(Vector3 vec){
		this.gridX = -1;
		this.gridY = -1;
		
		this.vec = vec;
	}
	
	public PositionMessage(int gx, int gy, Vector3 vec){
		this.gridX = gx;
		this.gridY = gy;

		this.vec.x = vec.x;
		this.vec.y = vec.y;
	}
}
