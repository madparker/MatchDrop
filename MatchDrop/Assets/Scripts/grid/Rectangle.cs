using UnityEngine;
using System.Collections;

public class Rectangle : MonoBehaviour {
	
	public Material mat;
	public Vector3[] vert;
	public bool drawable = true;

	public void SetUp(float width, float height){

		vert = new Vector3[4];
		
		vert[0] = new Vector3(-width/2,  height/2, -1);
		vert[1] = new Vector3( width/2,  height/2, -1);
		vert[2] = new Vector3( width/2, -height/2, -1);
		vert[3] = new Vector3(-width/2, -height/2, -1);

		Start ();

		if(drawable){
			transform.position = new Vector3(0, 0, 0);
		}
	}

	public bool containsa(float x, float y) {
		int hits = 0;

		int npoints = vert.Length;

		int lastx = (int)vert[npoints - 1].x;
		int lasty = (int)vert[npoints - 1].y;
		int curx, cury;
		
		// Walk the edges of the polygon
		for (int i = 0; i < npoints; lastx = curx, lasty = cury, i++) {
			curx = (int)vert[i].x;
			cury = (int)vert[i].y;
			
			if (cury == lasty) {
				continue;
			}
			
			int leftx;
			if (curx < lastx) {
				if (x >= lastx) {
					continue;
				}
				leftx = curx;
			} else {
				if (x >= curx) {
					continue;
				}
				leftx = lastx;
			}
			
			double test1, test2;
			if (cury < lasty) {
				if (y < cury || y >= lasty) {
					continue;
				}
				if (x < leftx) {
					hits++;
					continue;
				}
				test1 = x - curx;
				test2 = y - cury;
			} else {
				if (y < lasty || y >= cury) {
					continue;
				}
				if (x < leftx) {
					hits++;
					continue;
				}
				test1 = x - lastx;
				test2 = y - lasty;
			}
			
			if (test1 < (test2 / (lasty - cury) * (lastx - curx))) {
				hits++;
			}
		}
		
		return ((hits & 1) != 0);
	}

	public bool Contains(float x, float y) {
		int i;
		int j;
		bool result = false;
		for (i = 0, j = vert.Length - 1; i < vert.Length; j = i++) {
			if ((vert[i].y > y) != (vert[j].y > y) &&
			    (x < (vert[j].x - vert[i].x) * (y - vert[i].y) / (vert[j].y-vert[i].y) + vert[i].x)) {
				result = !result;
			}
		}
		return result;
	}

	void Start()
	{	
		//Create the triangles using the vertices
		int[] tris = new int[6];
		tris[0] = 0;
		tris[1] = 1;
		tris[2] = 2;
		tris[3] = 0;
		tris[4] = 2;
		tris[5] = 3;
		
		//Create a new mesh and pass down the vertices and triangles
		Mesh mesh = new Mesh();
		mesh.vertices = vert;
		mesh.triangles = tris;

		if(drawable){
			//Make sure mesh filter and mesh renderer componenets are attached
			if (!GetComponent<MeshFilter>())
				gameObject.AddComponent<MeshFilter>();
			
			if (!GetComponent<MeshRenderer>())
				gameObject.AddComponent<MeshRenderer>();
			
			//Pass down the mesh data to mesh filter
			gameObject.GetComponent<MeshFilter>().mesh = mesh;
			//Send material data to mesh renderer
			gameObject.GetComponent<MeshRenderer>().material = mat;
		}
	}
}
