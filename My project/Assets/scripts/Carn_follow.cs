using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carn_follow : MonoBehaviour
{
    [SerializeField]
    GameObject middle;

    [SerializeField]
    GameObject follow;

    

    Quaternion mid_rot;
    Quaternion move_change;

    float r = 10f;
    float speed = 0.1f;
    Vector3 mid;
    Vector3 moveTo;
    Vector3 oldPos;

    bool IsNewVector;

    private float time = 0.0f;
    public float interpolationPeriod = 0.01f;
    Vector3 R_vector;
    
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= interpolationPeriod)
        {
            
            
            time = 0;
        }

        //movement
        mid = transform.position - middle.transform.position;        
       
        oldPos = transform.position;
        MoveInDirection(moveTo, mid);
        R_vector = follow.transform.position;
        if (Vector3.Distance(R_vector, transform.position) <= 0.1)
        {
            moveTo = new Vector3(0, 0, 0);
        }
        else
        {
            moveTo = perpendicular((R_vector - transform.position), transform.position).normalized * speed;
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
        Gizmos.DrawLine(transform.position, follow.transform.position);

    }

    
}
    