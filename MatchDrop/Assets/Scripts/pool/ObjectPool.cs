using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
	
	public static bool prepopulate = true;
	public static GameObject objectPool;
	private static bool init = false;

	#region LinePool
	const string PREFAB_LINE = "Prefabs/GUI/Line";
	public static int numLines = 10;
	public static Queue<GameObject> lines;
	#endregion
	
	#region TokenPool
	const string PREFAB_TOKEN = "Prefabs/Token/Token";
	public static int numTokens = 68;
	public static Queue<GameObject> tokens;
	#endregion

	
//	GameObject points = (GameObject)Instantiate (Resources.Load ("Prefabs/GUI/Point Text", typeof(GameObject)));
	#region PointPool
	const string PREFAB_POINT = "Prefabs/GUI/Point Text";
	public static int numPoints = 68;
	public static Queue<GameObject> points;
	#endregion

	void Awake() {
		if(!init){
			init = true;
			DontDestroyOnLoad(gameObject);

			objectPool = this.gameObject;
			lines = new Queue<GameObject>();
			tokens = new Queue<GameObject>();
			points = new Queue<GameObject>();
			
			if(prepopulate){
				for(int i = 0; i < numTokens; i++){
					if(i < numTokens){
						RemoveToken(makeNewObject(PREFAB_TOKEN));
					}
					
					if(i < numPoints){
						RemovePoint(makeNewObject(PREFAB_POINT));
					}
					
					
					if(i < numLines){
						RemoveLine(makeNewObject(PREFAB_LINE));
					}
				}
			}
		} else {
			Destroy(gameObject);
		}
	}

	void Start ()
	{	

	}
	
	public static GameObject GetPoint(){
		GameObject point = GetFromPool(points, PREFAB_POINT);
		point.GetComponent<PointAnim>().Reset();

		return point;
	}
	
	public static void RemovePoint(GameObject point){
		AddToPool(point, points, numPoints);
	}
	
	public static GameObject GetLine(){
		return GetFromPool(lines, PREFAB_LINE);
	}
	
	public static void RemoveLine(GameObject line){
		AddToPool(line, lines, numLines);
	}
	
	public static GameObject GetToken(){
		GameObject token = GetFromPool(tokens, PREFAB_TOKEN);
		
		token.GetComponent<DisplayToken>().SetType(Random.Range(0, DisplayToken.MAX_TYPE));
		
		token.GetComponent<GridToken>().gen++;
		return token;
	}
	
	public static void RemoveToken(GameObject token){
		if(token.GetComponent<DisplayBombToken>() == null &&
		   token.GetComponent<DisplayZap>() == null &&
		   token.GetComponent<DisplayMultiToken>() == null &&
		   token.GetComponent<DisplayRockToken>() == null){
			token.GetComponent<DisplayToken>().StartUp();
			AddToPool(token, tokens, numTokens);
		} else {
			Destroy(token);
		}
	}

	private static GameObject makeNewObject(string prefab){
		return (GameObject)Instantiate(Resources.Load(prefab, typeof(GameObject)));
	}
	
	private static GameObject GetFromPool(Queue<GameObject> pool, string prefab){
		
		GameObject result = null;
		
		if(pool.Count != 0)
			result = pool.Dequeue();
		
		if(result == null){
			result = makeNewObject(prefab);
		}
		
		result.transform.parent = null;
		
		result.SetActive(true);
		result.GetComponent<Poolable>().Reset();
		
		return result;	
	}
	
	private static void AddToPool(GameObject obj, Queue<GameObject> pool, int size){
		if(pool.Count >= size){
			Destroy(obj);
		} else {
			obj.transform.parent = objectPool.transform;
			obj.SetActive(false);
			
			pool.Enqueue(obj);
		}
	}

}
