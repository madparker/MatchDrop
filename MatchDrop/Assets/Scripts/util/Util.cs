using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Util {


	public static string getFileContents(string fileName){
		//Open up a stream to a file to read from
		StreamReader reader = new StreamReader (fileName);
		
		//Read a line from the file
		string content = reader.ReadToEnd ();

		reader.Close();

		return content;
	}
	
	public static Vector3 CloneVector3(Vector3 vec){
		return new Vector3(vec.x, vec.y, vec.z);
	}
	
	public static Vector3 ModVector3(Vector3 vec, float x, float y, float z){
		return new Vector3(vec.x + x, vec.y + y, vec.z + z);
	}
	
	public static Vector3 ReplaceVector3X(Vector3 vec, float x){
		return new Vector3(x, vec.y, vec.z);
	}
	public static Vector3 ReplaceVector3Y(Vector3 vec, float y){
		return new Vector3(vec.x, y, vec.z);
	}
	public static Vector3 ReplaceVector3Z(Vector3 vec, float z){
		return new Vector3(vec.x, vec.y, z);
	}
}
