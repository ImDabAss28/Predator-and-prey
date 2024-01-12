using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Carn_follow : MonoBehaviour
{
    [SerializeField]
    GameObject middle;

	GameObject follow;
	Vector3 myvector;
	
    Quaternion mid_rot;
    Quaternion move_change;

    float r = 10f;
    float speed = 0.2f;
    float pregnant = 0.0f;
    float eaten = -100.0f;
    Vector3 mid;
    Vector3 moveTo;
    Vector3 oldPos;
    Vector3 random_factor;

    bool IsNewVector;



    private float time = 0.0f;
    public float interpolationPeriod = 0.01f;
    public float deathTime = 20.0f;
    private float deathtimevar = 0.0f;
    Vector3 R_vector;
    
    private void Start(){
		time = interpolationPeriod;
		myvector = new Vector3(0,0,0);
        pregnant = 0 - UnityEngine.Random.Range(0,1);
	}

    
    private void FixedUpdate()
    {
		if(pregnant > Time.deltaTime){
			pregnant -= Time.deltaTime;
		}
		else if(pregnant > 0){
			pregnant = 0;
			Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
		}
		GameObject[] Arr = GameObject.FindGameObjectsWithTag("carnivore");
		for(int i=0; i<Arr.Length; i++){
			if(1>Vector3.Distance(Arr[i].transform.position, transform.position) && pregnant == 0 && Time.realtimeSinceStartup-eaten < 20.0f){
				pregnant = 4.0f;
			}
		}
		
		time += Time.deltaTime;
		deathtimevar += Time.deltaTime;
		
		if (time >= interpolationPeriod)
		{
			random_factor = random_factor*0.4f + 0.6f * new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f));
			time = 0;
		}
		if(deathtimevar >= deathTime){
			Destroy(gameObject);
			eaten = Time.realtimeSinceStartup;
		}
	
		Arr = GameObject.FindGameObjectsWithTag("herbivore");
		follow = Arr[0];
		myvector = new Vector3(0,0,0);
		for(int i=0; i<Arr.Length; i++){
			if(Vector3.Distance(follow.transform.position, transform.position)>Vector3.Distance(Arr[i].transform.position, transform.position)){
				follow = Arr[i];
			}
			myvector += Arr[i].transform.position / Arr.Length;
		}

        //movement
        mid = transform.position - middle.transform.position;        
       
        oldPos = transform.position;
        MoveInDirection(moveTo, mid);
        R_vector = follow.transform.position;
        if (Vector3.Distance(R_vector, transform.position) <= 0.1)
        {
			Destroy(follow);
			deathtimevar = 0;
            moveTo = new Vector3(0, 0, 0);
        }
        else if(Vector3.Distance(R_vector, transform.position) > 1){
			//(new Vector3(Random.Range(-10.0f,10.0f),Random.Range(-10.0f,10.0f),Random.Range(-10.0f,10.0f))
			moveTo = perpendicular(((random_factor)*0.65f + 0.25f * (myvector - transform.position))+0.1f* (R_vector - transform.position), transform.position).normalized * speed;
		}
        else {
            //moveTo = perpendicular(((random_factor - transform.position)*0.20f + 0.35f * (myvector - transform.position))*(Vector3.Distance(R_vector, transform.position)/1.5f) + 0.9f* (1.5f-Vector3.Distance(R_vector, transform.position))/1.5f*(R_vector - transform.position)+transform.position, transform.position).normalized * speed;
            moveTo = perpendicular((random_factor)*0.05f+(R_vector-transform.position)*0.95f, transform.position).normalized * speed;
        }
        
    }

    Vector3 perpendicular(Vector3 a, Vector3 c)
    {
        Vector3 b = Vector3.ProjectOnPlane(a, Quaternion.AngleAxis(90f, transform.forward) * c);

        return b;
    }

    private void MoveInDirection(Vector3 a, Vector3 mid)
    {
        //mid_rot = Quaternion.Euler(mid);
        
        transform.position = (transform.position + moveTo).normalized * r;
        transform.LookAt(middle.transform.position);


    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (transform.position - moveTo * 10f));


    }


}
    
