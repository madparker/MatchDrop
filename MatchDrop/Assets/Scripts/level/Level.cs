using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Level 
{
	const string JSON_NEXT = "next";
	const string JSON_GRID = "grid";
	const string JSON_MSG = "message";

	public string nextInts;
	public string message;

	public char[,] charGrid = new char[GridHandler.GRID_WIDTH, GridHandler.GRID_HEIGHT];

	public Level(JSONNode json){
		nextInts = json[JSON_NEXT].Value;
		message = json[JSON_MSG].Value;

		JSONArray grid = json[JSON_GRID].AsArray;

		for(int y = 0; y < grid.Count; y++){
			char[] chars = grid[y].Value.ToCharArray();

			for(int x = 0; x < chars.Length; x++){
				charGrid[x, y] = chars[x];
			}
		}
	}

	public Level(string nexts, string[] lines){

		nextInts = nexts;

		for(int y = 0; y < lines.Length; y++){
			char[] chars = lines[y].ToCharArray();
			for(int x = 0; x < chars.Length; x++){
				charGrid[x, y] = chars[x];
			}
		}
	}
	
	public virtual void Activate(){
		
	}
	
	public virtual void Deactivate(){
		
	}
}
