using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    [SerializeField]
    private float _speed;

    private Vector3 _velocity;
    private Vector3 _friction = new Vector3(0.5f, 0, 0.5f);

    void Start()
    {
    }

    void Update()
    {
        Aim();
        Move();
    }

    void Move()
    {
        _velocity -= _friction * Time.deltaTime;
        transform.position += Time.deltaTime * _velocity * _speed;
    }

    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit dist;

        if (Physics.Raycast(ray, out dist))
        {
            if (dist.collider.gameObject.tag != "Player")
            {
                Vector3 hitPosition = ray.GetPoint(dist.distance);

                Vector3 clickPoint = new Vector3(hitPosition.x, transform.position.y, hitPosition.z);

                _velocity += (clickPoint - transform.position).normalized;

                transform.LookAt(clickPoint);
            }
        }
    }
}
