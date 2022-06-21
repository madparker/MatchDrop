using UnityEngine;
using System.Collections;

public class BlankTokenDisplay : MonoBehaviour {
	
	private float startSize = 0.5f;
	public bool inReplace = false;
	GridToken.POWERS power = GridToken.POWERS.POWER_NONE;
	int queuePos = -1;
	private static int counter = 0;

	private int preType = -1;

	public int PreType{
		set{
			preType = value;
		}
	}
	
	// Use this for initialization
	void Start () {
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = Resources.Load<Sprite>("images/tokens/special/blank");
		transform.localScale = new Vector3(startSize,startSize,startSize);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (inReplace) {
			FlipRemove();
		}
	}
	
	public void Replace(){
		inReplace = true;
	}
		
	public void Replace(GridToken.POWERS power, int queuePos){
		inReplace = true;
		this.power = power;
		this.queuePos = queuePos;
	}

	public void FlipRemove(){
		float rot = transform.rotation.y;

		if (rot < Mathf.PI / 4.5f) {
			transform.Rotate (Vector3.up * Time.deltaTime * DisplayToken.ROTATE_SPEED);
		} else {
			GameObject newToken;
			
			switch (power) {
			case GridToken.POWERS.POWER_COLOR_ZAP:
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenZap", typeof(GameObject)));
				break;
			case GridToken.POWERS.POWER_BOMB:
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/TokenBomb", typeof(GameObject)));
				break;
			case GridToken.POWERS.POWER_MULT_X1:
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/MultiToken", typeof(GameObject)));
				break;
			default:
				newToken = (GameObject)Instantiate(Resources.Load("Prefabs/Token/Token", typeof(GameObject)));
				break;
			}

			newToken.GetComponent<TokenMovement>().moveToken(transform.position.x, transform.position.y);
			newToken.transform.parent = transform.parent;

			counter++;

			if(power != GridToken.POWERS.POWER_COLOR_ZAP){
				int type = -1;

				if(preType > 0){
					type = preType;
				} else {
//					if(counter % 4 == 0){
//						type = GridHandler.GetSweetToken();
//					}

					if(type < 0){
						type = NextTokenHandler.GetNextFromBag();
					}
				}

				//TODO FIX FOR MILTI-TOKEN

				newToken.GetComponent<DisplayToken>().SetType(type);
			}

			if(queuePos > -1){
				NextTokenHandler.nextTokens.RemoveAt(queuePos);
				NextTokenHandler.nextTokens.Insert(queuePos, newToken.GetComponent<TokenMovement>());
			} else {
				newToken.GetComponent<TokenMovement>().Duplicate(GetComponent<TokenMovement>());
				GameManager.Next = newToken;
			}

			Destroy(gameObject);
		}
	}
}
