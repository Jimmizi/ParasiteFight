using UnityEngine;
using System.Collections.Generic;

public class Terrain : MonoBehaviour {
	
	public Sprite tex;
	public int width;
	public float res;
	public float rough;
	public float start;
	public float end;
	public Vector3 pos;
	
	// Use this for initialization
	void Start () {
		
		float[] heightmap;
		Vector3[] verts;
		int[] tris;
		Vector2[] uvs;
		
		heightmap = GenerateHeightMap (start, end, width);	
		verts = GenerateTerrain (heightmap, res);
		tris = triangulateHeightMap (verts.Length);
		uvs = GenerateUV (heightmap);
		
		GenerateMesh (verts, tris, uvs);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
	}
	
	float[] GenerateHeightMap(float startHeight, float endHeight, int count)
	{
		float[] heightmap;
		heightmap = new float[count + 1];
		heightmap[0] = startHeight;
		heightmap[heightmap.Length - 1] = endHeight;
		
		GenerateMidPoint(0, heightmap.Length - 1, rough, heightmap);
		
		return heightmap;
	}
	
	void GenerateMidPoint(int start, int end, float roughness, float[] heightmap) {
		
		int midPoint = (int)Mathf.Floor((start + end) / 2);
		
		if (midPoint != start) {
			
			float midHeight = (heightmap[start] + heightmap[end]) / 2;
			
			heightmap[midPoint] = midHeight + Random.Range(-roughness, roughness);
			
			GenerateMidPoint(start, midPoint, roughness / 2, heightmap);
			GenerateMidPoint(midPoint, end, roughness / 2, heightmap);
		}
	}
	
	Vector3[] GenerateTerrain(float[] heightmap, float resolution)
	{
		List<Vector3> vertices;
		vertices = new List<Vector3> ();
		res = Mathf.Max (1, resolution);
		
		for (int i = 0; i < heightmap.Length; i += 1) 
		{
			vertices.Add (new Vector3(i / resolution, heightmap[i], 0));
			vertices.Add (new Vector3(i / resolution, 0, 0));
		}
		
		return vertices.ToArray();
	}
	
	Vector2[] GenerateUV(float[] heightmap)
	{
		float inv = 1.0f / heightmap.Length;
		
		List<Vector2> uv;
		uv = new List<Vector2>();
		
		for (int i = 0; i < heightmap.Length; i++) {
			
			uv.Add(new Vector2((inv * i) * 20, heightmap[i] / 20));
			uv.Add(new Vector2((inv * i) * 20, 0));
			
		}
		
		Debug.Log (uv);
		
		
		return uv.ToArray();
	}
	
	int[] triangulateHeightMap(int numTri)
	{
		
		List<int> indices;
		indices = new List<int>();
		
		for (int i = 0; i <= numTri - 4; i += 2) {
			
			indices.Add(i);
			indices.Add(i + 3);
			indices.Add(i + 1);
			
			indices.Add(i + 3);
			indices.Add(i);
			indices.Add(i + 2);
			
		}
		
		return indices.ToArray();
	}
	
	void GenerateMesh(Vector3[] vertices, int[] tris, Vector2[] uvs)
	{
		
		Mesh mesh;
		mesh = new Mesh ();
		
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = tris;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		
		GameObject go;
		go = new GameObject ();
		
		go.AddComponent<MeshRenderer>();
		MeshFilter filter;
		filter = go.AddComponent<MeshFilter>();
		filter.mesh = mesh;
		
		go.renderer.material.mainTexture = tex.texture;
		
		go.transform.transform.position = pos;
		
		go.AddComponent<PolygonCollider2D> ();
		
	}
}