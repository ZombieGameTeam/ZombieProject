using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    void Start()
    {
    }


    void Update()
    {
        Aim();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward);
    }

    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit dist;

        if (Physics.Raycast(ray, out dist))
        {
            Vector3 clickPoint = new Vector3(ray.GetPoint(dist.distance).x, transform.position.y, ray.GetPoint(dist.distance).z);
            transform.LookAt(clickPoint);
        }
    }
}
