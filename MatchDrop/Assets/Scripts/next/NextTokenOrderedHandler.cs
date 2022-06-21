using UnityEngine;
using System.Collections;

public class NextTokenOrderedHandler : NextTokenHandler {

	string tokenOrder;	

	public void SetOrder(string tokenOrder){
		this.tokenOrder = tokenOrder;
		Setup();
	}

	// Use this for initialization
	public override void Setup () {
		base.Setup();

		nextTokens.Clear();

		foreach (Transform child in transform){
			Destroy (child.gameObject);
		}

		char[] chars = tokenOrder.ToCharArray();
		
		for(int i = 0; i < chars.Length; i++){
			
			GameObject newToken = GridPuzzleHandler.charToToken(chars[i]);	
			nextTokens.Add (newToken.GetComponent<TokenMovement>());

			newToken.GetComponent<TokenMovement>().moveToken(GridHandler.cols[0] - GridHandler.GRID_SIZE, GridHandler.rows[i] + offset);
			newToken.transform.parent = this.transform;	
		}
	}
	
	public override void Repopulate(){}
}
