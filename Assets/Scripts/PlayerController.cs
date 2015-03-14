using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    //Plane ground;
    void Start()
    {
        //ground = new Plane(Vector3.up, Vector3.zero);
    }


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit dist;

        if (Physics.Raycast(ray, out dist))
        {
            Vector3 clickPoint = new Vector3(ray.GetPoint(dist.distance).x, transform.position.y, ray.GetPoint(dist.distance).z);
            transform.LookAt(clickPoint);
        }
    }

    //void 
}
