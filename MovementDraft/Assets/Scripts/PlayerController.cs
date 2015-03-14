using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public float moveSpeed = 100.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Aim();
        Move();
	}

    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            Vector3 mousePosition = new Vector3(ray.GetPoint(hit.distance).x, transform.position.y, ray.GetPoint(hit.distance).z);
            transform.LookAt(mousePosition);
        }
    }
    void Move()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate((Vector3.forward) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate((Vector3.back) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate((Vector3.left) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate((Vector3.right) * moveSpeed * Time.deltaTime);
        }
    }
}
