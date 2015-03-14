using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Move();
	}

    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            Vector3 mousePosition = new Vector3(ray.GetPoint(hit.distance).x, transform.position.y, ray.GetPoint(hit.distance).z);
            Vector3 targetDir = mousePosition - transform.position;
            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
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
        if (Input.GetMouseButton(0))
        {
            Aim();
        }
    }
}
