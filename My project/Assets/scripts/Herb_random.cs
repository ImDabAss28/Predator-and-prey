using System.Collections;
using UnityEngine;

public class Herb_random : MonoBehaviour
{
    GlobalController controller;

    [SerializeField] GameObject middle;
    [SerializeField] GameObject child;

    Quaternion mid_rot;
    Quaternion move_change;

    float r = 10f;
    float speed = 0.15f;
    float pregnant = 0.0f;
    Vector3 mid;
    Vector3 moveTo;
    Vector3 oldPos;
    Vector3 myvector;

    public float interpolationPeriod = 3f;
    private float time;
    public float interpolationPeriodEaten = 0.01f;
    private float timeEaten = 0.0f;
    private float hord;
    
    // Triangles
    public GameObject tri;
    private RaycastHit hit;
    [SerializeField] private LayerMask triMask;
    triangle prevSR, SR;  

    Vector3 R_vector;

    private void Start()
    {
        Initialize();
    }

    private void Awake()
    {
        Initialize();
        pregnant = 0 - UnityEngine.Random.Range(0, 1);
    }

    //private void Update()
    //{
        //HandleInput();
    //}

    private void FixedUpdate()
    {
        HandleMovement();

        HandleTimeAndMitosis();

        HandleRandomVector();

        HandleHerbivoreInteractions();

        MoveToDestination();
        
        detTri();
    }

    private void Initialize()
    {
        controller = GetComponent<GlobalController>();
        time = interpolationPeriod;
        hord = UnityEngine.Random.Range(0.00f, 0.35f);
    }

    //private void HandleInput()
    //{
        //if (Input.GetKeyDown("h"))
        //{
            //controller.mitosis(this.gameObject, transform.position);
        //}
    //}

    private void HandleMovement()
    {
        mid = transform.position - middle.transform.position;
    }

    private void HandleTimeAndMitosis()
    {
        timeEaten += Time.deltaTime;

        if (pregnant > Time.deltaTime)
        {
            pregnant -= Time.deltaTime;
        }
        else if (pregnant > 0)
        {
            pregnant = 0;
            controller.mitosis(this.gameObject, transform.position);
        }
    }

    private void HandleRandomVector()
    {
        if (time >= interpolationPeriod)
        {
            R_vector = 0.35f * R_vector + 0.65f * new Vector3(
                UnityEngine.Random.Range(-10f, 10f),
                UnityEngine.Random.Range(-10f, 10f),
                UnityEngine.Random.Range(-10f, 10f)
            ).normalized * r;
            time = 0;
        }
    }

    private void HandleHerbivoreInteractions()
    {
        GameObject[] herbivores = GameObject.FindGameObjectsWithTag("herbivore");
        myvector = new Vector3(0, 0, 0);
        int cnt = 0;

        foreach (var herbivore in herbivores)
        {
            if (1 > Vector3.Distance(herbivore.transform.position, transform.position) && pregnant == 0)
            {
                pregnant = 10.0f;
            }

            if (3 > Vector3.Distance(herbivore.transform.position, transform.position))
            {
                myvector += herbivore.transform.position;
                cnt++;
            }
        }

        if (cnt > 0)
        {
            myvector /= cnt;
        }

        GameObject[] carnivores = GameObject.FindGameObjectsWithTag("carnivore");
        foreach (var carnivore in carnivores)
        {
            if (2 > Vector3.Distance(carnivore.transform.position, transform.position) && pregnant == 0)
            {
                myvector = (-carnivore.transform.position + transform.position) * 3 + transform.position;
            }
        }
    }

    private void MoveToDestination()
    {
        oldPos = transform.position;
        if (Vector3.Distance(((R_vector - transform.position) * (1.0f - hord) + (myvector - transform.position) * hord + transform.position), transform.position) <= 0.1)
        {
            moveTo = new Vector3(0, 0, 0);
        }
        else
        {
            moveTo = perpendicular(((R_vector - transform.position) * (1.0f - hord) + (myvector - transform.position) * hord), transform.position).normalized * speed;
        }

        transform.position = (transform.position + moveTo).normalized * r;
        transform.LookAt(middle.transform.position);
    }

    private Vector3 perpendicular(Vector3 a, Vector3 c)
    {
        Vector3 b = Vector3.ProjectOnPlane(a, Quaternion.AngleAxis(90f, transform.forward) * c);
        return b;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (transform.position - moveTo * 10f));
        Gizmos.DrawLine(transform.position, R_vector);
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
				prevSR.sheep.Remove(this.gameObject);
			}
			SR.sheep.Add(this.gameObject);
		}
	}
}
