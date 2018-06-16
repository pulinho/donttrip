using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MovementBase
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public GameObject carObject; // paint it black

    private float beginTime;

    private void Awake()
    {
        beginTime = Time.time;

        var rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, 500000));
    }

    private void Start()
    {
        carObject.SetColor(Color.gray);
    }

    public void FixedUpdate()
    {
        if(!isAlive)
        {
            return;
        }
        if (transform.position.y < -2.0f)
        {
            isAlive = false;
            return;
        }
        var torqueMultiplier = (Time.time - beginTime < 2) ? 1 // || (playerNumber > 0)
            : (Input.GetAxis("Fire" + playerNumber) - Input.GetAxis("Brake" + playerNumber));

        float motor = maxMotorTorque * torqueMultiplier;
        float steering = maxSteeringAngle * Input.GetAxis("LeftStickHorizontal" + playerNumber);

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
