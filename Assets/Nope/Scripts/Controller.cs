using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour {

	private Rigidbody rb;
	public Transform FL, FR, BL, BR;
	public float torque = 150;
	public float maxAngle = 30;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0, -.5f, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        // fuj fuj fuj
        if (transform.position.y < -30.0f)
        {
            SceneManager.LoadScene(0);
        }

        float forward = Input.GetAxis("Fire0");
        float backward = Input.GetAxis("Brake0");
        float steer = Input.GetAxis ("LeftStickHorizontal0");

        /*float forward = (Input.touchCount > 0) ? -1 : 1;
        float backward = 0.0f;
        float steer = GameManager.gyroTilt;*/

        // Debug.Log(rb.velocity.magnitude); //sqrMagnitude faster

        Vector3 ff = (Vector3.forward * torque * (forward - backward));
		//Vector3 ss = (Vector3.up * torque * 100 * steer);

        if(rb.velocity.sqrMagnitude < 600)
		    rb.AddRelativeForce (ff);
		//rb.AddRelativeTorque (ss);
		if (FL & FR) 
		{
			FR.localRotation = Quaternion.AngleAxis (steer * maxAngle, Vector3.up);
			FL.localRotation = Quaternion.AngleAxis (steer * maxAngle, Vector3.up);

		}
	}
}
