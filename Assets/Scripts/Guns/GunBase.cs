using UnityEngine;

public class GunBase : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public Color color;
    public float projectileSpeed;
    public float coolDownPeriodInSeconds;

    [HideInInspector] public int playerNumber = -1;
    [HideInInspector] public int bulletsLeft;

    private float coolDownTimeStamp = 0;

    void Start()
    {
        SetColor(color);
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

                var projectileInstance = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
                var projectileRigidBody = projectileInstance.GetComponent<Rigidbody>();
                projectileRigidBody.AddForce(spawnPoint.rotation * Vector3.up * projectileSpeed);
            }
        }
    }

    private void SetColor(Color color)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
}

