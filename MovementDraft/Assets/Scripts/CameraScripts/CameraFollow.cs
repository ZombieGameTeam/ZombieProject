using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public float smoothing = 5f;

    private Vector3 offset;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        offset = transform.position - target.position;
    }

    //void Start() {}

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

