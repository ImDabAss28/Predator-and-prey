using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Carn_follow : MonoBehaviour
{
    [SerializeField]
    GameObject middle;

    GameObject follow;
    Vector3 myvector;

    Quaternion mid_rot;
    Quaternion move_change;
    
    // Triangles
    public GameObject tri;
    private RaycastHit hit;
    [SerializeField] private LayerMask triMask;
    triangle prevSR, SR;  

    public int gender = 0;
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

    private void Start()
    {
        time = interpolationPeriod;
        myvector = new Vector3(0, 0, 0);
        gender = UnityEngine.Random.Range(0, 1);
    }

    private void FixedUpdate()
    {
        HandlePregnancy();
        HandleCarnivoreInteraction();
        HandleTimeAndDeath();

        UpdateRandomFactor();

        GameObject[] herbivores = GameObject.FindGameObjectsWithTag("herbivore");
        ChooseHerbivoreToFollow(herbivores);

        CalculateMovement();

        MoveInDirection(moveTo, mid);
        detTri();
    }

    // Handle pregnancy-related logic
    private void HandlePregnancy()
    {
        if (pregnant > Time.deltaTime)
        {
            pregnant -= Time.deltaTime;
        }
        else if (pregnant > 0)
        {
            pregnant = 0;
            Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    // Handle interactions with carnivores
    private void HandleCarnivoreInteraction()
    {
        GameObject[] carnivores = GameObject.FindGameObjectsWithTag("carnivore");
        foreach (var carnivore in carnivores)
        {
            if (1 > Vector3.Distance(carnivore.transform.position, transform.position) &&
                pregnant == 0 && Time.realtimeSinceStartup - eaten < 20.0f)
            {
                pregnant = 4.0f;
            }
        }
    }

    // Handle time and death logic
    private void HandleTimeAndDeath()
    {
        time += Time.deltaTime;
        deathtimevar += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            UpdateRandomFactor();
        }

        if (deathtimevar >= deathTime)
        {
            Destroy(gameObject);
            eaten = Time.realtimeSinceStartup;
        }
    }

    // Update random factor for movement
    private void UpdateRandomFactor()
    {
        random_factor = random_factor * 0.4f + 0.6f * new Vector3(
            UnityEngine.Random.Range(-1.5f, 1.5f),
            UnityEngine.Random.Range(-1.5f, 1.5f),
            UnityEngine.Random.Range(-1.5f, 1.5f)
        );
        time = 0;
    }

    // Choose herbivore to follow based on distance
    private void ChooseHerbivoreToFollow(GameObject[] herbivores)
    {
        follow = herbivores[0];
        myvector = new Vector3(0, 0, 0);

        foreach (var herbivore in herbivores)
        {
            if (Vector3.Distance(follow.transform.position, transform.position) >
                Vector3.Distance(herbivore.transform.position, transform.position))
            {
                follow = herbivore;
            }

            myvector += herbivore.transform.position / herbivores.Length;
        }
    }

    // Calculate movement logic
    private void CalculateMovement()
    {
        mid = transform.position - middle.transform.position;
        oldPos = transform.position;
        R_vector = follow.transform.position;

        if (Vector3.Distance(R_vector, transform.position) <= 0.1)
        {
            Destroy(follow);
            deathtimevar = 0;
            moveTo = new Vector3(0, 0, 0);
        }
        else if (Vector3.Distance(R_vector, transform.position) > 1)
        {
            moveTo = perpendicular(
                ((random_factor) * 0.65f + 0.25f * (myvector - transform.position)) +
                0.1f * (R_vector - transform.position),
                transform.position
            ).normalized * speed;
        }
        else
        {
            moveTo = perpendicular(
                (random_factor) * 0.05f + (R_vector - transform.position) * 0.95f,
                transform.position
            ).normalized * speed;
        }
    }

    // Calculate perpendicular vector
    private Vector3 perpendicular(Vector3 a, Vector3 c)
    {
        Vector3 b = Vector3.ProjectOnPlane(a, Quaternion.AngleAxis(90f, transform.forward) * c);
        return b;
    }

    // Move in a specified direction
    private void MoveInDirection(Vector3 a, Vector3 mid)
    {
        transform.position = (transform.position + moveTo).normalized * r;
        transform.LookAt(middle.transform.position);
    }

    // Draw gizmos for debugging
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (transform.position - moveTo * 10f));
    }
    
    private void detTri()
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
				prevSR.wolves.Remove(this.gameObject);
			}
			SR.wolves.Add(this.gameObject);
		}
	}
}
