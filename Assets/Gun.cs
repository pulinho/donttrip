using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject projectilePrefab;
    public Transform spawnPoint;
    [HideInInspector] public int playerNumber = -1;

    private float coolDownTimeStamp = 0;

    private const float coolDownPeriodInSeconds = 0.2f;
    private const float projectileSpeed = 300.0f;

    // Use this for initialization
    void Start () {
        setColor(Color.black);
    }
	
	// Update is called once per frame
	void Update () {

        if(playerNumber == -1)
        {
            return;
        }

        if (Input.GetAxis("Fire" + playerNumber) > 0.9f)
        {
            //Debug.Log("Fire" + playerNumber);
            if (coolDownTimeStamp <= Time.time)
            {
                coolDownTimeStamp = Time.time + coolDownPeriodInSeconds;

                var projectileInstance = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
                var projectileRigidBody = projectileInstance.GetComponent<Rigidbody>();
                projectileRigidBody.AddForce(spawnPoint.rotation * Vector3.up * projectileSpeed);
            }
        }
    }

    private void setColor(Color color) // make extension
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = color;
        }
    }

    /*public void Fire()
    {
        if (coolDownTimeStamp <= Time.time)
        {
            var m_Instance = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
            coolDownTimeStamp = Time.time + coolDownPeriodInSeconds;
        }
    }*/
}
