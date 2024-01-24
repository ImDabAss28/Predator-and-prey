using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class CameraMovement : MonoBehaviour
{
    private float speed = 1000f;
    void Update()
    {
        if (Input.GetKey("b"))
        {
            CamOrbit();
        }

    }

    private void CamOrbit()
    {
        float verticalInput = -Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        transform.Rotate(Vector3.right, verticalInput);
        transform.Rotate(Vector3.up, horizontalInput, Space.World);
    }
}
