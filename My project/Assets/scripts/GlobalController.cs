using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public void mitosis(GameObject origin, Vector3 p)

    {
        GameObject obj;

        obj = Instantiate(origin, p, origin.transform.rotation);
        

        if (obj.CompareTag("herbivore"))
        {
            
            Herbivores.Add(obj);
            
        }
        else
        if (obj.CompareTag("carnivore"))
        {
            Carnivores.Add(obj);
        }

        
    }

    public List<GameObject> Carnivores;
    public List<GameObject> Herbivores;
    public static GameObject toUse;
    public static int function = 0;
    bool init = false;

    GameObject[] Arr;



    private void Update()
    {

    


    }

    private float r = 10f;

	private void Start(){
		if(Time.realtimeSinceStartup < 10){
			if(GameObject.FindGameObjectsWithTag("herbivore").Length < 30){
				mitosis(GameObject.FindGameObjectsWithTag("herbivore")[0], GameObject.FindGameObjectsWithTag("herbivore")[0].transform.position);
			}
			if(GameObject.FindGameObjectsWithTag("carnivore").Length < 10){
				mitosis(GameObject.FindGameObjectsWithTag("carnivore")[0], GameObject.FindGameObjectsWithTag("carnivore")[0].transform.position);
			}
		}
	}
}
