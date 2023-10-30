using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = Quaternion.AngleAxis(1f, transform.forward) * transform.position;
            Debug.Log(transform.position);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = Quaternion.AngleAxis(-1f, transform.forward) * transform.position;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = Quaternion.AngleAxis(1f, transform.up) * transform.position;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = Quaternion.AngleAxis(-1f, transform.up) * transform.position;
        }


    }
}
