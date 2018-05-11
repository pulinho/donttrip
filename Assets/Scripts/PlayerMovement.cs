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

    [HideInInspector] public int playerNumber = 0;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public Transform gunPoint;

    private GunBase equippedGun;

    void OnCollisionEnter(Collision col)
    {
        collisionCount++;

        if(!isAlive || equippedGun != null)
        {
            return;
        }
        var gun = col.gameObject.GetComponent<GunBase>(); // todo: tag or something maybe?
        if (gun != null && gun.isActive)
        {
            TakeGun(gun);
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
    
    void Start () {
        verticalAxisName = "LeftStickVertical" + playerNumber;
        horizontalAxisName = "LeftStickHorizontal" + playerNumber;
    }
	
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

        if(angle > 85.0f && collisionCount > 0)
        {
            if (deathTimeStamp == 0)
            {
                deathTimeStamp = Time.time + deathTimeoutSec;
            }
            else if(deathTimeStamp <= Time.time)
            {
                Die();
                return;
            }
        }
        else
        {
            deathTimeStamp = 0;
        }

        if (transform.position.y < -2.0f)
        {
            Die();
            return;
        }

        if(equippedGun != null && !equippedGun.isActive)
        {
            DropGun();
        }

        Move(Mathf.Cos(Mathf.Deg2Rad * angle));
    }

    private void Die()
    {
        isAlive = false;
        DropGun();
    }

    private void TakeGun(GunBase gun)
    {
        var rb = gun.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.detectCollisions = false;
            rb.isKinematic = true;
        }
        gun.transform.SetPositionAndRotation(gunPoint.position, gunPoint.rotation);
        gun.transform.parent = gunPoint;
        gun.playerNumber = playerNumber;

        equippedGun = gun;
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
        }
        equippedGun.transform.parent = null;
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

        if(Input.GetButton("Jump" + playerNumber) && collisionCount > 0 && jumpCoolDownTimeStamp <= Time.time)
        {
            jumpCoolDownTimeStamp = Time.time + jumpCoolDownPeriodSec;
            bodyRigidbody.AddForce(new Vector3(0, 350 * angleMultiplier, 0));
        }

        if (Input.GetButton("DropGun" + playerNumber)) {
            DropGun();
        }
    }
}
