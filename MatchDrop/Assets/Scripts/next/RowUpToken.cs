using UnityEngine;
using System.Collections;

public class RowUpToken : MonoBehaviour {
	
	private float startSize = 0.5f;
	
	public int mode;

	public bool hasStep = false;
	
	const int MODE_DEFAULT = 0;
	const int MODE_SLIDE_DOWN = 1;
	const int MODE_SLIDE_UP = 2;
	const int MODE_REMOVE = 3;

	public static int rockIndex = -1;

	// Use this for initialization
	void Start () {
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = Resources.Load<Sprite>("images/tokens/special/rowUp");
		transform.localScale = new Vector3(startSize, startSize, startSize);
		
		mode = MODE_DEFAULT;
	}
	
	// Update is called once per frame
	void Update () {

		if (mode != MODE_DEFAULT) {
			transform.position = new Vector3 (
				transform.position.x,
				transform.position.y,
				-1);
		}

		if(!hasStep){
			if(mode == MODE_SLIDE_DOWN && GetComponent<TokenMovement>().atDestination() && !GridHandler.inAnim()){
	//			Step();
				hasStep = true;
				Invoke ("Step", 0.25f);
	//			GridHandler.rowUp();
			} else if(mode == MODE_SLIDE_UP  && !GridHandler.inAnim()){
				Step();
			} else if(mode == MODE_REMOVE  && GetComponent<TokenMovement>().atDestination()){
				if(!GridHandler.gameOver){
					NextTokenHandler nth = (NextTokenHandler)FindObjectOfType(typeof(NextTokenHandler));
					nth.addLevelUpToken();
					
					Destroy(gameObject);
				}
			}
		}
	}

	public void Step(){
		mode++;
	
		hasStep = false;

		switch(mode){
		case(MODE_SLIDE_DOWN):
			GetComponent<TokenMovement>().setNewDest(GridHandler.cols[0] - GridHandler.GRID_SIZE, 
		    GridHandler.rows[GridHandler.GRID_HEIGHT - 1] + GridHandler.GRID_SIZE, -1);
//				GridHandler.rowUp();
			
			if(GridHandler.level % 5 == 0 && DisplayToken.MAX_TYPE < DisplayToken.sprites.Length){
				DisplayToken.MAX_TYPE++;
			}
			break;
		case(MODE_SLIDE_UP):
			GameManager.getGameManager ().GetComponent<GameManager> ().playRowUpSound();

			if(DisplayToken.MAX_TYPE == 7){
				rockIndex = Random.Range(0, GridHandler.GRID_WIDTH);
			} else {
				rockIndex = -1;
			}

			GridHandler.rowUp();

			if(!GridHandler.gameOver){
				GameManager.Next = GameManager.nextTokenHandler.GetComponent<NextTokenHandler>().getNextToken().gameObject;
			}
			GridHandler.level++;
			GameManager.updateScoreLevelDelegate();
		    GetComponent<TokenMovement>().setNewDest(
				GridHandler.cols[0] - GridHandler.GRID_SIZE, 
				GridHandler.rows[GridHandler.GRID_HEIGHT - 1],
				-1);
			break;
			case(MODE_REMOVE):
				break;
			default:
				break;
		}
	}
}
