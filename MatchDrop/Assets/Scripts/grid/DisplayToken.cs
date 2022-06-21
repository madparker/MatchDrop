using UnityEngine;
using System.Collections;

[RequireComponent (typeof (TokenMovement))]
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (GridToken))]
public class DisplayToken : MonoBehaviour {
	
	public static Sprite[] sprites;
	public static int MAX_TYPE;
	public int type;
	public float size;
	public float startTime = Mathf.PI/6;
	
	public static float startSize = 0.5f;
	
	public static bool isInit = false;
	
	public static bool inAnim = false;
	
	public static float SHRINK_SPEED = 4.0f;
	public GridToken gridToken;

	public const float ROTATE_SPEED = 400; 
	
	void Setup(){
		if(!isInit){
			Debug.Log("INIT DISPLAY");

			isInit = true;
			sprites = Resources.LoadAll<Sprite>("images/tokens/standard");
			MAX_TYPE = 4;
		}
	}
	
	// Use this for initialization
	void Awake () {
		StartUp();
	}
	
	public virtual void StartUp(){
		Setup();

		transform.localRotation = new Quaternion();

		startTime = Mathf.PI/6;
		size = startSize; 
		transform.localScale = new Vector3(size,size,size);
		
		type = (int)Random.Range(0, MAX_TYPE);
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = sprites[type];

		gameObject.GetComponent<GridToken>().marked = false;
	}

	public void SetType(int i){
		size = startSize; 
		transform.localScale = new Vector3(size,size,size);
		
		type = i;
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = sprites[type];
	}

	void Start(){
		transform.Rotate (Vector3.down * Mathf.PI/4.5f * ROTATE_SPEED);
	}
	
	// Update is called once per frame
	void Update () {
		Display();
		UpdateRot ();
	}

	public void UpdateRot(){
		float rot = transform.rotation.y;
		
		if (rot < 0) {
			transform.Rotate (Vector3.down * Time.deltaTime * ROTATE_SPEED);
		} else {
			transform.localRotation = new Quaternion();
		}
	}

	public virtual void Display(){

		if(gridToken == null){
			gridToken = gameObject.GetComponent<GridToken>();
		}
		
		if(size > 0 && gridToken.marked){
			
			Vector3 fore = Util.CloneVector3(transform.position);
//			fore.z = -1;
			transform.position = fore;
			
			transform.Rotate (Vector3.down * Time.deltaTime * ROTATE_SPEED * 2);

			startTime += Time.deltaTime * SHRINK_SPEED;

//			Debug.Log(Mathf.Sin(startTime));

			inAnim = true;
//			size -= SHRINK_SPEED * Time.deltaTime;
			size = Mathf.Sin(startTime) * .75f;
			transform.localScale = new Vector3(size,size,size);
		} else if(size <= 0){
			inAnim = false;
			gridToken.remove();
		}
	}
}
