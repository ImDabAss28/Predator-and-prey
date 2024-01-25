using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
	float pregnant = 0.0f;
	float r = 10.0f;
	float die = -1.0f;
	bool init = false;
	bool dest = false;
	int initCnt = 0;
	
	// Triangles
    public GameObject tri;
    private RaycastHit hit;
    [SerializeField] private LayerMask triMask;
    [SerializeField] float timeMult = 3.0f;
    triangle prevSR, SR;

    // Update is called once per frame
    void FixedUpdate()
    {
		if(Vector3.Distance(Vector3.zero, transform.position)>1){
			if(die < 0) die = UnityEngine.Random.Range(2.0f, 5.0f);
			detTri();
			if(initCnt > 10){
				if(SR != null) SR.plants.Remove(this.gameObject);
				Destroy(this.gameObject);
			}
			HandleDeath();
		}
		if(init || Vector3.Distance(Vector3.zero, transform.position)<1){
			HandleMitosis();
		}
    }
    
    private void HandleMitosis()
    {
        if (pregnant > Time.deltaTime) pregnant -= Time.deltaTime;
        else if (pregnant != 0) {
            pregnant = 0;
            initCnt = 0;
            mitosis(this.gameObject, randomPos());
        }
        else pregnant = UnityEngine.Random.Range(timeMult, timeMult+1.25f);
    }
    
    private void HandleDeath(){
		if(die > Time.deltaTime){
			if(dest) die -= Time.deltaTime;
		}
		else{
			SR.plants.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
		if(SR != null && SR.sheep.Count > 0){
			dest = true;
		}
	}
    
    private Vector3 randomPos()
    {
        Vector3 R_vector = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        ).normalized * r;
        return R_vector;
    }
    
	private void detTri()
	{
		if(!init) initCnt++;
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
				prevSR.plants.Remove(this.gameObject);
			}
			SR.plants.Add(this.gameObject);
			init = true;
		}
	}
	
    private void mitosis(GameObject origin, Vector3 p)
    {
        GameObject obj = Instantiate(origin, p, origin.transform.rotation);
    }	
}
