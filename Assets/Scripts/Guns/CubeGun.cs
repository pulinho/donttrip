using UnityEngine;

public class CubeGun : GunBase
{
    private void Awake()
    {
        //coolDownPeriodInSeconds = 0.4f;
        //projectileSpeed = 300.0f;
        bulletsLeft = 5;
        color = Color.magenta;
    }
}
