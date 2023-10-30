using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{
    

    [SerializeField]
    GameObject middle;
    Quaternion mid_rot;
    Quaternion move_change;

    float r = 10f;
    float speed = 0.15f;
    Vector3 mid;
    Vector3 moveTo;
    Vector3 oldPos;

    bool IsNewVector;

    private float time = 0.0f;
    public float interpolationPeriod = 5f;
    Vector3 R_vector;
    private void Start()
    {
        R_vector = new Vector3(0, 0, 0);
    
    }

    private void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            moveTo = transform.up * speed;
        }
        if (Input.GetKeyDown("s"))
        {
            moveTo = -transform.up * speed;
        }
        if (Input.GetKeyDown("a"))
        {
            moveTo = -transform.right * speed;
        }
        if (Input.GetKeyDown("d"))
        {
            moveTo = transform.right * speed;
        }
        if (Input.GetKeyDown("e"))
        {
            moveTo = (transform.up + transform.right).normalized * speed;
        }
        if (Input.GetKeyDown("q"))
        {
            moveTo = (transform.up - transform.right).normalized * speed;
        }
        if (Input.GetKeyDown("y"))
        {
            moveTo = (-transform.up -transform.right).normalized * speed;
        }
        if(Input.GetKeyDown("x")) 
        {
            moveTo = (-transform.up + transform.right).normalized * speed;
        }
    }
    private void FixedUpdate()
    {
        

        mid = transform.position - middle.transform.position;
        //movement
        
        oldPos = transform.position;
        MoveInDirection(moveTo, transform.position);
        moveTo = Quaternion.FromToRotation(oldPos, transform.position) * (moveTo); ;

        





        //move_change = Quaternion.FromToRotation(oldPos, transform.position);
        //if (IsNewVector == false){
        //    R_vector = move_change * R_vector;}
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
        Gizmos.DrawLine(transform.position, (transform.position - moveTo * 10f)  );
        //Gizmos.DrawLine(middle.transform.position, );

    }
    Quaternion CalRotation(Vector3 oldPos, Vector3 newPos)
    {
        Vector3 b, c, d, e;
        Quaternion a;

        b = perpendicular(oldPos, Vector3.up);
        c = perpendicular(newPos, Vector3.up);



        d = perpendicular(oldPos, Vector3.forward);
        e = perpendicular(newPos, Vector3.forward);

        a = Quaternion.Euler(Vector3.Angle(b, c), Vector3.Angle(d, e), 0 );

        return a;
    }
}
