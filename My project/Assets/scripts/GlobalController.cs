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
    
    [SerializeField] int carn = 1;
    [SerializeField] int herb = 1;
    [SerializeField] bool rand = false;
    [SerializeField] bool topdown = false;
    
    float r = 10.0f;
    public static bool startupdone = false;

    private void Start()
    {
        if(!startupdone) onStartup();
    }

	private void onStartup(){
		GameObject[] herbivores = GameObject.FindGameObjectsWithTag("herbivore");
		GameObject[] carnivores = GameObject.FindGameObjectsWithTag("carnivore");
		if(rand){
			if(topdown){
				if (herbivores.Length < herb) mitosis(herbivores[0], randomLocDown());
				if (carnivores.Length < carn) mitosis(carnivores[0], randomLocUp());
			}
			else{
				if (herbivores.Length < herb) mitosis(herbivores[0], randomLoc());
				if (carnivores.Length < carn) mitosis(carnivores[0], randomLoc());
			}
		}
		else{
			if (herbivores.Length < herb) mitosis(herbivores[0], herbivores[0].transform.position);
			if (carnivores.Length < carn) mitosis(carnivores[0], carnivores[0].transform.position);
		}
		if (herbivores.Length >= herb && carnivores.Length >= carn) startupdone = true;
	}

	Vector3 randomLoc(){
		Vector3 R_vector = new Vector3(
			UnityEngine.Random.Range(-1f, 1f),
			UnityEngine.Random.Range(-1f, 1f),
			UnityEngine.Random.Range(-1f, 1f)
		).normalized * r;
		return R_vector;
	}
	
	Vector3 randomLocUp(){
		Vector3 R_vector = new Vector3(
			UnityEngine.Random.Range(-1f, 1f),
			UnityEngine.Random.Range(0.7f, 1f),
			UnityEngine.Random.Range(-1f, 1f)
		).normalized * r;
		return R_vector;
	}
	
	Vector3 randomLocDown(){
		Vector3 R_vector = new Vector3(
			UnityEngine.Random.Range(-1f, 1f),
			UnityEngine.Random.Range(-1f, -0.7f),
			UnityEngine.Random.Range(-1f, 1f)
		).normalized * r;
		return R_vector;
	}

    public void mitosis(GameObject origin, Vector3 p)
    {
        GameObject obj = Instantiate(origin, p, origin.transform.rotation);
    }
}
