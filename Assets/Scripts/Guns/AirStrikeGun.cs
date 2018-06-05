using UnityEngine;

public class AirStrikeGun : GunBase
{
    private void Awake()
    {
        bulletsLeft = 1;
    }

    protected override void Fire()
    {
        for(int i=0; i<4; i++)
        {
            var actualSpawn = transform.position + (Vector3.up * 50f) + (Vector3.up * i * 2.5f) + Random.insideUnitSphere;
            var projectileInstance = Instantiate(projectilePrefab, actualSpawn, Quaternion.identity) as GameObject;

            projectileInstance.SetColor(Color.black);
            var projectileRigidBody = projectileInstance.GetComponent<Rigidbody>();

            projectileRigidBody.AddForce(Vector3.down * 500f);
            projectileRigidBody.AddTorque(Random.insideUnitSphere * 50);
        }

        bulletsLeft = 0;
        Deactivate();
    }
}
