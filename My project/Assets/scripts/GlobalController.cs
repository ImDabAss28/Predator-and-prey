using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalController : MonoBehaviour
{

    public List<GameObject> Carnivores;
    public List<GameObject> Herbivores;
    public static GameObject toUse;
    public static int function = 0;

    GameObject[] Arr;

    private void Update()
    {

    


    }

    private float r = 10f;

    public void FindCarnivores()
    {
        float a = 0.3f;
        Arr = GameObject.FindGameObjectsWithTag("carnivore");
        foreach (GameObject carnivore in Arr)
        {

            Carnivores.Add(carnivore);

        }

        Arr = new GameObject[0];
    }

    private void FindHerbivores()
    {
        float a = 3 * r;
        Arr = GameObject.FindGameObjectsWithTag("herbivore");
        foreach (GameObject herbivore in Arr)
        {
            Herbivores.Add(herbivore);

        }
        Arr = new GameObject[0];
    }
    public void mitosis(GameObject origin)

    {
        GameObject obj;
        
        obj = Instantiate(origin, origin.transform.position, origin.transform.rotation);
        

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
}
