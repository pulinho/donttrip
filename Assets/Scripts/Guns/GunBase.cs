using UnityEngine;

public class GunBase : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public Color color;
    public Color projectileColor;
    public float projectileSpeed;
    public float coolDownPeriodInSeconds;

    [HideInInspector] public int playerNumber = -1;
    [HideInInspector] public int bulletsLeft;
    [HideInInspector] public bool isActive = true;

    private float coolDownTimeStamp = 0;

    void Start()
    {
        gameObject.SetColor(color);
    }

    void Update()
    {
        if (playerNumber == -1)
        {
            return;
        }

        if (Input.GetAxis("Fire" + playerNumber) > 0.9f)
        {
            if (coolDownTimeStamp <= Time.time)
            {
                coolDownTimeStamp = Time.time + coolDownPeriodInSeconds;
                Fire();
            }
        }
    }

    private void Fire()
    {
        var projectileInstance = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
        //projectileInstance.SetColor(projectileColor);
        projectileInstance.SetColor(Random.ColorHSV());

        var projectileRigidBody = projectileInstance.GetComponent<Rigidbody>();
        projectileRigidBody.AddForce(spawnPoint.rotation * Vector3.up * projectileSpeed);
        projectileRigidBody.AddTorque(Random.insideUnitSphere * 10);

        bulletsLeft--;
        if(bulletsLeft == 0)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        isActive = false;
        gameObject.SetColor(Color.white);
    }
}

