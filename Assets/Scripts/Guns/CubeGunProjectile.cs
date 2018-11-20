using UnityEngine;

public class CubeGunProjectile : MonoBehaviour {

    public GameObject subProjectilePrefab;
    public Transform[] subSpawn;

    private bool isBlown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBlown)
        {
            isBlown = true;
            BlowUp();
        }
    }

    private void BlowUp()
    {
        foreach (var spawn in subSpawn)
        {
            var subProjectileInstance = Instantiate(subProjectilePrefab, spawn.position, spawn.rotation) as GameObject;
            subProjectileInstance.SetColor(Random.ColorHSV(0, 1, 1, 1));

            var subProjectileRigidBody = subProjectileInstance.GetComponent<Rigidbody>();
            subProjectileRigidBody.AddForce(spawn.rotation * new Vector3(0, 0, 1) * 500f);
            subProjectileRigidBody.AddTorque(Random.insideUnitSphere * 20);
        }

        Object.Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
