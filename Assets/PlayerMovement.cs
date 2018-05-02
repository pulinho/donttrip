using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private const float speed = 500f;

    private string verticalAxisName;
    private string horizontalAxisName;

    private float horizontalMovement;
    private float verticalMovement;

    private Rigidbody bodyRigidbody;

    private int collisionCount = 0;

    private float jumpCoolDownTimeStamp = 0;
    private const float jumpCoolDownPeriodSec = 0.5f;

    private float deathTimeStamp = 0;
    private const float deathTimeoutSec = 2.0f;

    [HideInInspector] public int m_PlayerNumber = 0;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public Transform gunPoint;

    private Gun equippedGun;

    void OnCollisionEnter(Collision col)
    {
        collisionCount++;

        if(equippedGun != null)
        {
            return;
        }
        var gun = col.gameObject.GetComponent<Gun>();
        if (gun != null)
        {
            var rb = gun.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.detectCollisions = false;
                rb.isKinematic = true;

                gun.transform.SetPositionAndRotation(gunPoint.position, gunPoint.rotation);
                gun.transform.parent = gunPoint;
            }

            gun.playerNumber = m_PlayerNumber;
            equippedGun = gun;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCount--;
    }

    private void Awake()
    {
        bodyRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        isAlive = true;
        // When the tank is turned on, make sure it's not kinematic.
        bodyRigidbody.isKinematic = false;

        // Also reset the input values.
        horizontalMovement = 0f;
        verticalMovement = 0f;
    }

    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        bodyRigidbody.isKinematic = true;
    }

    // Use this for initialization
    void Start () {
        verticalAxisName = "Vertical" + m_PlayerNumber;
        horizontalAxisName = "Horizontal" + m_PlayerNumber;
    }
	
	// Update is called once per frame
	void Update () {
        horizontalMovement = Input.GetAxis(horizontalAxisName);
        verticalMovement = Input.GetAxis(verticalAxisName);
    }

    private void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }

        var angle = Vector3.Angle(Vector3.up, transform.up);
        //Debug.Log(m_PlayerNumber + " :" + angle);

        if(angle > 85.0f && collisionCount > 0)
        {
            if (deathTimeStamp == 0)
            {
                deathTimeStamp = Time.time + deathTimeoutSec;
            }
            else if(deathTimeStamp <= Time.time)
            {
                isAlive = false;
                return;
            }
        }
        else
        {
            deathTimeStamp = 0;
        }

        if (transform.position.y < -2.0f)
        {
            isAlive = false;
            return;
        }

        Move(Mathf.Cos(Mathf.Deg2Rad * angle));
        //Move(1.0f - (angle / 90.0f));
    }

    private void DropGun()
    {
        if(equippedGun == null)
        {
            return;
        }
        var rb = equippedGun.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.detectCollisions = true;
            rb.isKinematic = false;

            //gun.transform.SetPositionAndRotation(gunPoint.position, gunPoint.rotation);
            equippedGun.transform.parent = null;
        }

        equippedGun.playerNumber = -1;
        equippedGun = null;
    }

    private void Move(float angleMultiplier)
    {
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            var moveDirection = new Vector3(horizontalMovement, 0, verticalMovement) * speed * Time.deltaTime * angleMultiplier;
            bodyRigidbody.AddForce(moveDirection);
        }

        if(Input.GetButton("Jump" + m_PlayerNumber) && collisionCount > 0 && jumpCoolDownTimeStamp <= Time.time)
        {
            jumpCoolDownTimeStamp = Time.time + jumpCoolDownPeriodSec;

            bodyRigidbody.AddForce(new Vector3(0, 350 * angleMultiplier, 0)); // make it relative?
        }

        if (Input.GetButton("DropGun" + m_PlayerNumber)) {
            DropGun();
        }
    }

    //Turn?
}
