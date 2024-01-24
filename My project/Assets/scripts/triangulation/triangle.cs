using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class triangle : MonoBehaviour
{
	
    public List<GameObject> neighbors = new List<GameObject>();
    public List<GameObject> sheep = new List<GameObject>();
    public List<GameObject> wolves = new List<GameObject>();
    public Vector3 pos;
    public Vector3[] vertices = new Vector3[3];

    void Start()
    {
        Transform tri = GetComponent<Transform>();
        Mesh mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        for(int i = 0; i < 3; i++)
        {
            vertices[i] = tri.transform.TransformPoint(vertices[i]);
        }
        pos = (vertices[0] + vertices[1] + vertices[2]) / 3;
        
    }


}
