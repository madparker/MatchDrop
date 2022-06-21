using UnityEngine;
using System.Collections;

public class DisplayNumberToken : DisplayToken {

	MeshRenderer numHolder, rect;
	TextMesh text;
	public int num;
	public Material mat;

	public void Setup () {

		foreach (Transform child in transform){
			if(child.gameObject.name == "numHolder"){
				text = child.gameObject.GetComponent<TextMesh>();
				numHolder = child.gameObject.GetComponent<MeshRenderer>();
			} else if(child.gameObject.name == "rect"){
				rect = child.gameObject.GetComponent<MeshRenderer>();
			}
		}
	}
	
	public override void Display(){
		base.Display();

		if(num > 0){
			inAnim = false;
		}
	}

	public void setNumber(float n){

		float f = Random.Range(0, n);
		
		if(f - (int)f < 0.25){
			num = 0;
		} else {
			num = (int)Mathf.Floor(f);
		}
		
		if(num > 9){
			num = 9;
		}

		num = 0;

		UpdateGUI(num);
	}

	public void UpdateGUI(int i) {
		Setup();

		num = i;

		if(num <= 0){
			numHolder.enabled = false;
			rect.enabled = false;
		} else {
			text.text = "" + i;
		}
	}
}
