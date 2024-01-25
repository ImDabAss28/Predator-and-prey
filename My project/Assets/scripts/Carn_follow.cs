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
    Vector3 Goal;

    Quaternion mid_rot;
    Quaternion move_change;
    
    // Triangles
    public GameObject tri;
    private RaycastHit hit;
    [SerializeField] private LayerMask triMask;
    triangle prevSR, SR;  

    public bool gender = false;
    float PregTime = 10.0f;
    float r = 10f;
    float speed = 0.2f;
    float pregnant = 0.0f;
    float eaten = -100.0f;
    float ranFact = 0.0f;
    float lastMove = 0.0f;
    
    Vector3 mid;
    Vector3 moveTo;
    Vector3 oldPos;
    Vector3 R_vector;
    Vector3 Dest = Vector3.zero;

    bool IsNewVector;

    private float time = 0.0f;
    public float interpolationPeriod = 0.01f;
    public float deathTime = 30.0f;
    private float deathtimevar = 0.0f;
    bool danger = false;
    bool in_rad = false;
    bool init = false;
    int initCnt = 0;
    
    float timeEaten = 0.0f;

    private void Start()
    {
		Initialize();
        Goal = new Vector3(0, 0, 0);
    }
    
    private void Initialize()
    {
		lastMove = Time.realtimeSinceStartup;
		speed = UnityEngine.Random.Range(0.13f, 0.18f);
        time = interpolationPeriod;
        gender = (1 == UnityEngine.Random.Range(0, 2));
        time = interpolationPeriod;
    }

    private void FixedUpdate()
    {
		if(GlobalController.startupdone){
			detTri();
			if(initCnt > 10){
				if(SR != null) SR.wolves.Remove(this.gameObject);
				Destroy(this.gameObject);
			}
			if(init){
				initCnt = 0;
				Death();
				ChooseHerbivoreToFollow();
				CalculateMovement();
				MoveInDirection();
				HandleMitosis();
			}
		}
    }

    private void HandleMitosis()
    {
        timeEaten += Time.deltaTime;
 		foreach (var carnivore in SR.wolves) {
			if(gender && !carnivore.GetComponent<Carn_follow>().gender && pregnant == 0.0){
				pregnant = PregTime;
			}
		}
		
		if (pregnant > Time.deltaTime) pregnant -= Time.deltaTime;
        else if (pregnant != 0) {
            pregnant = 0;
            init = false;
            initCnt = 0;
            mitosis(this.gameObject, transform.position);
        }
    }

    private void Death()
    {
        time += Time.deltaTime;
        deathtimevar += Time.deltaTime;

        if (deathtimevar >= deathTime)
        {
            SR.wolves.Remove(this.gameObject);
            Destroy(gameObject);
            eaten = Time.realtimeSinceStartup;
        }
    }

    private void RandomFactor()
    {
        R_vector = R_vector * 0.3f + 0.7f * new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        ).normalized * r;
        time = 0;
    }

    // Choose herbivore to follow based on distance
    private void ChooseHerbivoreToFollow()
    {
        Goal = Vector3.zero;
        follow = null;
        int cnt = 0;

		if(SR.sheep.Count == 0){
			in_rad = false;
			//danger = false;
			foreach(var tri in SR.neighbors){
				foreach(var herbivore in tri.GetComponent<triangle>().sheep){
					danger = true;
					Goal += herbivore.transform.position - transform.position;
					cnt++;
				}
			}
			if(cnt != 0) Goal = Goal / cnt;
			//if(!danger)
			Goal = Goal * UnityEngine.Random.Range(0.7f, 0.9f);
		}
		else{
			follow = SR.sheep[0];
			Goal = SR.sheep[0].transform.position - transform.position;
			danger = true;
			in_rad = true;
		}
		if(Goal.magnitude < 0.1) danger = false;
    }

    // Calculate movement logic
    private void CalculateMovement()
    {
		if (time >= interpolationPeriod){
			RandomFactor();
			ranFact = UnityEngine.Random.Range(0.10f, 0.15f);
			time = 0;
		}
		


		if (in_rad && Vector3.Distance(Dest, transform.position) < 0.15)
		{
			SR.sheep.Remove(follow);
			Destroy(follow);
			deathtimevar = 0;
			Dest = transform.position;
			time = interpolationPeriod;
			in_rad = false;
		}
		else if(in_rad){
			Dest = follow.transform.position;
		}
		else if (danger)
		{
			Dest = Goal * (1 - ranFact) + R_vector * (ranFact);
		}
		else
		{
			Dest = (R_vector + ranFact*Goal) / (1+ranFact);
		}
        Dest = Dest.normalized * r;
		if(Vector3.Distance(Dest, transform.position) < 0.20){
			time = interpolationPeriod;
			Dest = transform.position;
		}

        moveTo = perpendicular(Dest - transform.position, transform.position).normalized * speed;
        if(danger) moveTo = moveTo * UnityEngine.Random.Range(1.0f, 1.15f);
    }

    private Vector3 perpendicular(Vector3 a, Vector3 c)
    {
        Vector3 b = Vector3.ProjectOnPlane(a, Quaternion.AngleAxis(90f, transform.forward) * c);
        return b;
    }

    private void MoveInDirection()
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
		if(!init) Initialize();
		initCnt++;
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
			init = true;
		}
	}
	
    private void mitosis(GameObject origin, Vector3 p)
    {
        GameObject obj = Instantiate(origin, p, origin.transform.rotation);
    }	
}
