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
        maxPlayerDistance = 10.0f;

        foreach(var target in targets)
        {
            var bias = Camera.main.aspect;
            var biasVec = new Vector3(1 / bias, 1, 1);

            var distance = Vector3.Distance(Vector3.Scale(averagePlayerPosition, biasVec), Vector3.Scale(target.position, biasVec));
            //var distance = Vector3.Distance(averagePlayerPosition, target.position);
            if (distance > maxPlayerDistance)
            {
                maxPlayerDistance = distance;
            }
        }
        
        offset = (new Vector3(0, 2.1f, -1.4f) * maxPlayerDistance);
    }

    private void FindAveragePosition()
    {
        Vector3 minVector = new Vector3(100, 0, 100);
        Vector3 maxVector = new Vector3(-100, 0, -100);

        foreach(var target in targets)
        {
            if (!target.gameObject.activeSelf)
            {
                continue;
            }
            
            if(target.position.x < minVector.x)
            {
                minVector.x = target.position.x;
            }
            if (target.position.z < minVector.z)
            {
                minVector.z = target.position.z;
            }
            if (target.position.x > maxVector.x)
            {
                maxVector.x = target.position.x;
            }
            if (target.position.z > maxVector.z)
            {
                maxVector.z = target.position.z;
            }
        }

        averagePlayerPosition = minVector + ((maxVector - minVector) / 2);
    }
}
