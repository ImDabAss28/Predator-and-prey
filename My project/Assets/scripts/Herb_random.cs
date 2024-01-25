using System.Collections;
using UnityEngine;

public class Herb_random : MonoBehaviour
{
    GlobalController controller;
    [SerializeField] GameObject middle;
    [SerializeField] GameObject child;
    [SerializeField] float fa;
    [SerializeField] float fb;

    Quaternion mid_rot;
    Quaternion move_change;

    float flocking = 0;
    bool danger = false;
    float r = 10f;
    float speed = 0.15f;
    int initCnt = 0;
    
    public bool gender = false; // 0 male 1 female
    float pregnant = 0.0f;
    float PregTime = 10.0f;
    
    Vector3 moveTo = Vector3.zero;
    Vector3 avgSpeed = Vector3.zero;
    Vector3 oldPos = Vector3.zero;
    Vector3 MidPoint = Vector3.zero;
    Vector3 Escape = Vector3.zero;
    Vector3 Dest = Vector3.zero;

    public float interpolationPeriod = 3f;
    private float time;
    public float interpolationPeriodEaten = 0.01f;
    private float timeEaten = 0.0f;
    
    // Triangles
    public GameObject tri;
    private RaycastHit hit;
    [SerializeField] private LayerMask triMask;
    triangle prevSR, SR;
    bool init = false;

    Vector3 R_vector;

    private void Start()
    {
        controller = GetComponent<GlobalController>();
		Initialize();
        Escape = Vector3.zero;
    }

    private void FixedUpdate()
    {
		if(GlobalController.startupdone){
			detTri();
			if(initCnt > 10){
				if(SR != null) SR.sheep.Remove(this.gameObject);
				Destroy(this.gameObject);
			}
			if(init){
				initCnt = 0;
				RandomFactor();
				HandleHerbivoreInteractions();
				MoveToDestination();
				HandleMitosis();
			}
		}	
    }

    private void Initialize()
    {
		timeEaten = UnityEngine.Random.Range(15.0f, 25.0f);
		speed = UnityEngine.Random.Range(0.10f, 0.15f);
        gender = (1 == UnityEngine.Random.Range(0,2));
        flocking = UnityEngine.Random.Range(fa, fb);
        time = interpolationPeriod;
    }

    private void HandleMitosis()
    {
        timeEaten -= Time.deltaTime;
        if(timeEaten < 0.0f){
			SR.sheep.Remove(this.gameObject);
			Destroy(this.gameObject);
		}

        if (pregnant > Time.deltaTime) pregnant -= Time.deltaTime;
        else if (pregnant != 0) {
            pregnant = 0;
            init = false;
            initCnt = 0;
            controller.mitosis(this.gameObject, transform.position);
        }
    }

    private void RandomFactor()
    {
		if (Vector3.Distance(perpendicular((MidPoint - transform.position) * flocking + R_vector * (1 - flocking) + Escape, transform.position).normalized  + transform.position, transform.position) < 0.5)
        {
            time = interpolationPeriod;
        }
        
		time += Time.deltaTime;
        if (time >= interpolationPeriod )
        {
			UpdateFlockingVecs();
			float oldRatio = UnityEngine.Random.Range(0.05f, 0.15f);
            R_vector = oldRatio * R_vector + (1-oldRatio) * new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
            ).normalized * r;
            time = 0;
        }
    }

	private void UpdateFlockingVecs(){
        MidPoint = new Vector3(0, 0, 0);
        int cnt = 0;
        
		foreach (var herbivore in SR.sheep) {
			avgSpeed += herbivore.GetComponent<Herb_random>().moveTo;
			MidPoint += herbivore.transform.position;
			cnt++;
		}
		foreach (var tri in SR.neighbors){
			foreach(var herbivore in tri.GetComponent<triangle>().sheep){
				avgSpeed += herbivore.GetComponent<Herb_random>().moveTo;
				MidPoint += herbivore.transform.position;
				cnt++;
			}
		}
        MidPoint /= cnt;
        avgSpeed /= cnt;
	}

    private void HandleHerbivoreInteractions()
    {
		foreach (var herbivore in SR.sheep) {
			if(gender && !herbivore.GetComponent<Herb_random>().gender && pregnant == 0.0){
				pregnant = PregTime;
			}
		}
		
		if(SR.plants.Count > 0){
			R_vector = 0.10f * R_vector + 0.90f * SR.plants[0].GetComponent<Plant>().transform.position;
			timeEaten = UnityEngine.Random.Range(15.0f, 25.0f);
		}
		else{
			foreach(var tri in SR.neighbors){
				if(tri.GetComponent<triangle>().plants.Count > 0){
					R_vector = 0.10f * R_vector + 0.90f * tri.GetComponent<triangle>().plants[0].GetComponent<Plant>().transform.position;
					MidPoint = MidPoint * 0.4f + 0.6f * tri.GetComponent<triangle>().plants[0].GetComponent<Plant>().transform.position;
					avgSpeed = avgSpeed * 0.7f + 0.3f * (tri.GetComponent<triangle>().plants[0].GetComponent<Plant>().transform.position - transform.position);
				}
			}
		}

		
		if(SR.wolves.Count == 0){
			foreach(var tri in SR.neighbors){
				foreach(var carnivore in tri.GetComponent<triangle>().wolves){
					Escape = transform.position - carnivore.transform.position;
					danger = true;
				}
			}
			if(!danger) Escape = Escape * UnityEngine.Random.Range(0.3f, 0.8f);
		}
		else{
			Escape = transform.position - SR.wolves[0].transform.position;
			danger = true;
		}
		if(Escape.magnitude < 0.01) danger = false;
        
    }

    private void MoveToDestination()
    {
        oldPos = transform.position;
        
        
        
        if(danger){
			Dest = Escape * 0.95f + R_vector * 0.05f;
		}
		else
        {
			Dest = MidPoint * flocking * 0.2f + (avgSpeed + transform.position) * flocking * 0.8f + R_vector * Mathf.Abs(1 - flocking);
        }
        Dest = Dest.normalized * r;
        if(Vector3.Distance(Dest, transform.position) < 0.15){
			moveTo = Vector3.zero;
			time = interpolationPeriod;
		}
		
        moveTo = perpendicular(Dest - transform.position, transform.position).normalized*speed;
        if(danger) moveTo = moveTo * UnityEngine.Random.Range(1.0f, 1.15f);
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
				prevSR.sheep.Remove(this.gameObject);
			}
			SR.sheep.Add(this.gameObject);
			init = true;
		}
	}
}
