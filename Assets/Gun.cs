using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject projectilePrefab;
    public Transform spawnPoint;
    [HideInInspector] public int playerNumber = -1;

    private float coolDownTimeStamp = 0;

    private const float coolDownPeriodInSeconds = 0.2f;
    private const float projectileSpeed = 300.0f;
    
    void Start ()
    {
        SetColor(Color.black);
    }
	
	void Update ()
    {
        if(playerNumber == -1)
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
