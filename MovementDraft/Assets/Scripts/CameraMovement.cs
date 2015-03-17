using UnityEngine;
using System.Collections;
using System;

public class CameraMovement : MonoBehaviour 
{
    public float smooth = 1.5f;

    private float cameraAdjustment = 0.5f;
    private Transform player;
    private Vector3 relativeCameraPosition; //Relative to the Player
    private float relativeCameraPositionMagnitude; //Distance, as float, from camera to player
    private Vector3 newPosition;

    private Vector3 yCorrection; //Camera gets too close. This is a workaround, needs to be fixed

    void Start() { yCorrection = new Vector3(0, 0, 0); }

    void FixedUpdate()
    {
        float moveStep = 2 * smooth * Time.deltaTime;
        float rotationStep = smooth * Time.deltaTime;
        /* The following three should not be here */
        player = GameObject.FindGameObjectWithTag("Player").transform; //Must save in Start or Awake, but for this I must ensure this runs after player instantiation.
        relativeCameraPosition = transform.position - player.position;
        relativeCameraPositionMagnitude = relativeCameraPosition.magnitude - cameraAdjustment;
        yCorrection.y = 1/relativeCameraPositionMagnitude;

        Vector3 standardPosition = player.position + relativeCameraPosition;
        Vector3 abovePosition = player.position + Vector3.up * relativeCameraPositionMagnitude;
        Vector3[] checkPoints = new Vector3[5];
        checkPoints[0] = standardPosition;
        checkPoints[1] = Vector3.Lerp(standardPosition, abovePosition, 0.15f);
        checkPoints[2] = Vector3.Lerp(standardPosition, abovePosition, 0.20f);
        checkPoints[3] = Vector3.Lerp(standardPosition, abovePosition, 0.25f);
        checkPoints[4] = abovePosition;

        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (canView(checkPoints[i])) { break; }
        }

        transform.position = Vector3.MoveTowards(transform.position, player.position, moveStep);
        transform.position = Vector3.Lerp(transform.position, newPosition, rotationStep) + yCorrection;
        SmoothLookAt();
        //Bad
        //transform.position = new Vector3( player.position.x, transform.position.y, player.position.y);
    }

    bool canView(Vector3 positionToCheck)
    {
        RaycastHit hit;
        if(Physics.Raycast(positionToCheck,player.position-positionToCheck,out hit,relativeCameraPositionMagnitude))
        {
            if(hit.transform != player)
            {
                return false;
            }
        }
        newPosition = positionToCheck;
        return true;
    }
    void SmoothLookAt()
    {
        Vector3 relativePlayerPosition = player.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(relativePlayerPosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation,smooth * Time.deltaTime);
    }
}
