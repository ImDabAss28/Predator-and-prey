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
    Vector3 mid;
    Vector3 moveTo;
    Vector3 oldPos;

    bool IsNewVector;

    public float interpolationPeriod = 3f;
    private float time;
    public float interpolationPeriodEaten = 0.01f;
    private float timeEaten = 0.0f;

    Vector3 R_vector;
    private void Start()
    {
        controller = GetComponent<GlobalController>();
        time = interpolationPeriod;
    }
    private void Awake()
    {
        controller = GetComponent<GlobalController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown("h"))
        {

            
            controller.mitosis(this.gameObject);

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

        if (time >= interpolationPeriod)
        {
            R_vector = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f)).normalized * r;
            time = 0;
        }


        oldPos = transform.position;
        MoveInDirection(moveTo, mid);
        if (Vector3.Distance(R_vector, transform.position) <= 0.1)
        {
            moveTo = new Vector3(0, 0, 0);
        }
        else
        {
        moveTo = perpendicular((R_vector - transform.position), transform.position).normalized * speed;

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
