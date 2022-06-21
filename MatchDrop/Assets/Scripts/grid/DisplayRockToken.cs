using UnityEngine;
using System.Collections;

public class DisplayRockToken : DisplayToken {

	// Use this for initialization
	void Start () {
		type = -5;
	}

	public override void StartUp(){
		size = startSize; 
	}

	public override void Display () {
		base.Display();
	}
}
