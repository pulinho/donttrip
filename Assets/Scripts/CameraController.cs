using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public Transform[] targets;
    public float dampTime = 0.2f;

    private Vector3 offset;
    private Vector3 moveVelocity;
    private Vector3 averagePlayerPosition;
    private float maxPlayerDistance;

    private void FixedUpdate()
    {
        FindAveragePosition();
        
        Zoom();
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.SmoothDamp(transform.position, averagePlayerPosition + offset, ref moveVelocity, dampTime);
    }

    private void Zoom()
    {
        maxPlayerDistance = 5.0f;

        foreach(var target in targets)
        {
            var distance = Vector3.Distance(averagePlayerPosition, target.position);
            if (distance > maxPlayerDistance)
            {
                maxPlayerDistance = distance;
            }
        }
        
        offset = (new Vector3(0, 2.4f, -1.6f) * maxPlayerDistance);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        foreach(var target in targets)
        {
            if (!target.gameObject.activeSelf)
            {
                continue;
            }
            
            averagePos += target.position;
            numTargets++;
        }

        if (numTargets > 0)
        {
            averagePos /= numTargets;
        }

        averagePlayerPosition = averagePos;
    }
}
