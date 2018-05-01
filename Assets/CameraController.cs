using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public Transform[] targets;
    public float dampTime = 0.2f;

    private Vector3 offset;// = new Vector3(0, 0.9f, -0.6f);//new Vector3(0, 15, -10);
    private Vector3 moveVelocity;
    private Vector3 averagePlayerPosition;
    private float maxPlayerDistance;

    /*void LateUpdate() // late??
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = desiredPosition + offset;
    }*/

    private void FixedUpdate()
    {
        FindAveragePosition();

        // Change the size of the camera based.
        Zoom();

        // Move the camera towards a desired position.
        Move();
    }

    private void Move()
    {
        // Smoothly transition to that position.
        transform.position = Vector3.SmoothDamp(transform.position, averagePlayerPosition + offset/*+ (offset * maxPlayerDistance)*/, ref moveVelocity, dampTime);
    }

    private void Zoom()
    {
        maxPlayerDistance = 5.0f;

        for (int i = 0; i < targets.Length; i++)
        {
            var distance = Vector3.Distance(averagePlayerPosition, targets[i].position);
            if(distance > maxPlayerDistance)
            {
                maxPlayerDistance = distance;
            }
        }

        //Debug.Log("MaxPlaye" + maxPlayerDistance);
        offset = (new Vector3(0, 2.4f, -1.6f) * maxPlayerDistance);// - averagePlayerPosition;
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < targets.Length; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!targets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += targets[i].position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        //averagePos.y = transform.position.y;

        // The desired position is the average position;
        averagePlayerPosition = averagePos;
    }
}
