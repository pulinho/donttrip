using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public int m_PlayerNumber = 0;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;

    private string verticalAxisName;
    private string horizontalAxisName;

    private float horizontalMovement;
    private float verticalMovement;

    private Rigidbody m_Rigidbody;

    private int collisionCount = 0;

    private float jumpCoolDownTimeStamp = 0;
    private const float jumpCoolDownPeriodSec = 0.5f;

    private float deathTimeStamp = 0;
    private const float deathTimeoutSec = 3.0f;

    public bool isAlive;

    void OnCollisionEnter(Collision col)
    {
        collisionCount++;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCount--;
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        isAlive = true;
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        horizontalMovement = 0f;
        verticalMovement = 0f;
    }

    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
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


    private void Move(float angleMultiplier)
    {
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            var moveDirection = new Vector3(horizontalMovement, 0, verticalMovement) * m_Speed * Time.deltaTime * angleMultiplier;
            m_Rigidbody.AddForce(moveDirection);
        }

        if(Input.GetButton("Jump" + m_PlayerNumber) && collisionCount > 0 && jumpCoolDownTimeStamp <= Time.time)
        {
            jumpCoolDownTimeStamp = Time.time + jumpCoolDownPeriodSec;

            m_Rigidbody.AddForce(new Vector3(0, 350 * angleMultiplier, 0)); // make it relative?
        }
    }

    //Turn?
}
