using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.Rendering;

public class findNeighbors : MonoBehaviour
{
   
    triangle scriptRef, scriptRef2;
    List<GameObject> triangles = new List<GameObject>();
    [HideInInspector] static bool doneTri = false;
    void Start()
    {
		doneTri = false;
        foreach (Transform t in transform)
        {
            t.gameObject.tag = "triangle";
            t.gameObject.layer = 3;
            t.gameObject.AddComponent<MeshCollider>();
            t.gameObject.AddComponent<triangle>();
            triangles.Add(t.gameObject);
            
        }
	}
	
	void Update(){
		if(doneTri == false){
			foreach (GameObject tri1 in triangles)
			{
				scriptRef = tri1.GetComponent<triangle>();
				foreach (GameObject tri2 in triangles)
				{
					int ex = 0;
					scriptRef2 = tri2.GetComponent<triangle>();
					if (tri1 != tri2)
					{
						for (int i = 0; i < 3; i++) {
							for (int j = 0; j < 3; j++) {
								if (scriptRef.vertices[i] == scriptRef2.vertices[j])
								{
									scriptRef.neighbors.Add(tri2);
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
			
			Debug.Log("finished");
			Debug.Log(triangles.Count);
			doneTri = true;
		}
    }
}
