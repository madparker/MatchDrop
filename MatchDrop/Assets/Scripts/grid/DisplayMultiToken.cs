using UnityEngine;
using System.Collections;

public class DisplayMultiToken : DisplayToken {
	
	Material mat1;
	Material mat2;

	float angle = 0;
	PolygonCollider2D poly;

	Shader shader1;
	public int type2;
	
	public override void StartUp () {
		base.StartUp();
		type2 = (int)Random.Range(0, MAX_TYPE);
		while(type2 == type){
			type2 = (int)Random.Range(0, MAX_TYPE);
		}

		SpriteRenderer sr1 = GetComponent<SpriteRenderer>();
		mat1 = sr1.material;
		
		foreach (Transform child in transform){
			if(child.gameObject.GetComponent<SpriteRenderer>() != null){
				SpriteRenderer sr2 = child.gameObject.GetComponent<SpriteRenderer>();
				sr2.sprite = sprites[type2];
				mat2 = sr2.material;
			}
		}
	}
	
	public void SetTypes(int i1, int i2){
		size = startSize; 
		transform.localScale = new Vector3(size,size,size);
		
		type = i1;
		type2 = i2;
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = sprites[type];

		SpriteRenderer sr2 = transform.GetChild(0).GetComponent<SpriteRenderer>();
		sr2.sprite = sprites[type2];
	}
	
	// Update is called once per frame
	void Update () {
		Display();
		UpdateRot ();
		/*RotateRect();

		texture = (Texture2D)mat1.GetTexture("_AlphaTex");
		
		i++;
		
		if(i == texture.height){
			i = 0;
		}
		
		int y = 0;
		while (y < texture.height) {
			int x = 0;
			while (x < texture.width) {
				
				Color color = new Color (1, 0, 1, 0);
				
				if(rect.containsa(x, y)){
					color = new Color (1, 1, 1, 1);
				}
				
				texture.SetPixel(x, y, color);
				++x;
			}
			++y;
		}
		texture.Apply();*/

		angle += 1f * Time.deltaTime;
		
		mat1.SetVector("_Vector1",  new Vector4(Mathf.Sin(angle), Mathf.Cos(angle), 0, 0));
		mat2.SetVector("_Vector1",  new Vector4(Mathf.Sin(angle), Mathf.Cos(angle), 0, 0));
		//mat1.SetVector("_Vector2",  new Vector4(Mathf.Sin(Mathf.PI-angle), Mathf.Cos(Mathf.PI-angle), 0, 0));
		
		//mat1.SetTexture("_AlphaTex", texture);
		//mat2.SetTexture("_AlphaTex", texture);
	}
}
