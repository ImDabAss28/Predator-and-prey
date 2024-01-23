using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.Rendering;

public class findNeighbors : MonoBehaviour
{
   
    triangle scriptRef, scriptRef2;
    List<GameObject> triangles = new List<GameObject>();
    void Start()
    {
        
        foreach (Transform t in transform)
        {
            t.gameObject.tag = "triangle";
            t.gameObject.layer = 3;
            t.gameObject.AddComponent<MeshCollider>();
            t.gameObject.AddComponent<triangle>();
            triangles.Add(t.gameObject);
            
        }


    }
    void Update() 
    {
            
        if (Input.GetKeyUp("h"))
        {
            foreach (GameObject triangle in triangles)
            {
                scriptRef = triangle.GetComponent<triangle>();
                foreach (GameObject triangle2 in triangles)
                {
                    int ex = 0;
                    scriptRef2 = triangle2.GetComponent<triangle>();
                    if (triangle != triangle2)
                    {
                        for (int i = 0; i < 3; i++) {
                            for (int j = 0; j < 3; j++) {
                                if (Vector3.Distance(scriptRef.vertices[i], scriptRef2.vertices[j]) < 0.01)
                                {
                                    scriptRef.neighbors.Add(triangle2);
                                    ex = 1;
                                    break;
                                }
                            }
                            if (ex == 1)
                            {
                                break;
                            }    
                        }
                    }
                }
            }
        }                
    }
}
