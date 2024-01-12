using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Herb_random : MonoBehaviour
{

    GlobalController controller;


    [SerializeField]
    GameObject middle;



    [SerializeField]
    GameObject child;

    

    Quaternion mid_rot;
    Quaternion move_change;

    float r = 10f;
    float speed = 0.15f;
    float pregnant = 0.0f;
    Vector3 mid;
    Vector3 moveTo;
    Vector3 oldPos;
    Vector3 myvector;

    bool IsNewVector;

    public float interpolationPeriod = 3f;
    private float time;
    public float interpolationPeriodEaten = 0.01f;
    private float timeEaten = 0.0f;
    private float hord;

    Vector3 R_vector;
    private void Start()
    {
        controller = GetComponent<GlobalController>();

        time = interpolationPeriod;
        hord = UnityEngine.Random.Range(0.00f, 0.35f);
    }
    private void Awake()
    {
        controller = GetComponent<GlobalController>();
        pregnant = 0 - UnityEngine.Random.Range(0,1);
    }
    private void Update()
    {
		
		
        if (Input.GetKeyDown("h"))
        {

            controller.mitosis(this.gameObject, transform.position);

        }


    }

    private void FixedUpdate()
    {
        //movement
        mid = transform.position - middle.transform.position;

        timeEaten += Time.deltaTime;
        if(timeEaten >= interpolationPeriodEaten)
        {
            
        }
        time += Time.deltaTime;
        if(pregnant > Time.deltaTime){
			pregnant -= Time.deltaTime;
		}
		else if(pregnant > 0){
			pregnant = 0;
			controller.mitosis(this.gameObject, transform.position);
		}

        if (time >= interpolationPeriod)
        {
            R_vector = 0.35f * R_vector + 0.65f * new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f)).normalized * r;
            time = 0;
        }
        
		GameObject[] Arr = GameObject.FindGameObjectsWithTag("herbivore");
		myvector = new Vector3(0,0,0);
		int cnt = 0;
		for(int i=0; i<Arr.Length; i++){
			if(1>Vector3.Distance(Arr[i].transform.position, transform.position) && pregnant == 0){
				pregnant = 10.0f;
			}
			if(3>Vector3.Distance(Arr[i].transform.position, transform.position)){
				myvector += Arr[i].transform.position;
				cnt++;
			}
		}
		myvector /= cnt;
		
		Arr = GameObject.FindGameObjectsWithTag("carnivore");
		for(int i=0; i<Arr.Length; i++){
			if(2>Vector3.Distance(Arr[i].transform.position, transform.position) && pregnant == 0){
				myvector = (- Arr[i].transform.position + transform.position)*3 + transform.position;
			}
		}

        oldPos = transform.position;
        MoveInDirection(moveTo, mid);
        if (Vector3.Distance(((R_vector - transform.position)*(1.0f-hord) + (myvector - transform.position)  * hord+transform.position), transform.position) <= 0.1)
        {
            moveTo = new Vector3(0, 0, 0);
        }
        else
        {
        moveTo = perpendicular(((R_vector - transform.position)*(1.0f-hord) + (myvector -transform.position) * hord), transform.position).normalized * speed;

        }
        
    }

    private Vector3 perpendicular(Vector3 a, Vector3 c)
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
        Gizmos.DrawLine(transform.position, R_vector);

    }


}
