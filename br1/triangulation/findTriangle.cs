using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findTriangle : MonoBehaviour
{
    public GameObject tri;
    private RaycastHit hit;

    [SerializeField] private LayerMask triMask;

    triangle prevSR, SR;

    int start;
    private void Start()
    {
        start = 0;
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown("h"))
        {
            start = 1;
        }

        if (start == 1)
        {
            Vector3 raycastDir = Vector3.zero - transform.position;
            Physics.Raycast(transform.position, raycastDir, out hit, 10f, triMask);
            //Debug.DrawRay(transform.position, raycastDir, Color.red, 10f);
            if (hit.collider != null && tri != hit.transform.gameObject)
            {
                tri = hit.transform.gameObject;
                prevSR = SR;
                SR = tri.GetComponent<triangle>();
                if(prevSR != null)
                {
                    prevSR.sheep.Remove(this.gameObject);
                }
                SR.sheep.Add(this.gameObject);

                Debug.Log(tri);
            }
        }
        
    }

}
