using UnityEngine;

public class BallGun : GunBase {

    private void Awake()
    {
        coolDownPeriodInSeconds = 0.2f;
        projectileSpeed = 300.0f;
        bulletsLeft = 20;
        //color = Color.black;
    }
}
