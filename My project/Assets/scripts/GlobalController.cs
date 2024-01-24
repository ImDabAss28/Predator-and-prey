using System.Collections;
using System.Collections.Generic;
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
        // Any global update logic can be added here
    }

    private void Start()
    {
        // Initializations and mitosis calls can be moved to separate methods for better organization
        Initialize();
        PerformMitosis();
    }

    private void Initialize()
    {
        // Initializations can go here
    }

    private void PerformMitosis()
    {
        // Perform mitosis based on certain conditions
        if (Time.realtimeSinceStartup < 10)
        {
            PerformHerbivoreMitosis();
            PerformCarnivoreMitosis();
        }
    }

    private void PerformHerbivoreMitosis()
    {
        GameObject[] herbivores = GameObject.FindGameObjectsWithTag("herbivore");

        if (herbivores.Length < 0)
        {
            mitosis(herbivores[0], herbivores[0].transform.position);
        }
    }

    private void PerformCarnivoreMitosis()
    {
        GameObject[] carnivores = GameObject.FindGameObjectsWithTag("carnivore");

        if (carnivores.Length < 0)
        {
            mitosis(carnivores[0], carnivores[0].transform.position);
        }
    }

    public void mitosis(GameObject origin, Vector3 p)
    {
        GameObject obj = Instantiate(origin, p, origin.transform.rotation);

        if (obj.CompareTag("herbivore"))
        {
            Herbivores.Add(obj);
        }
        else if (obj.CompareTag("carnivore"))
        {
            Carnivores.Add(obj);
        }
    }
}
