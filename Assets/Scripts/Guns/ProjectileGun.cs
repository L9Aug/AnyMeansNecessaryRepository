using UnityEngine;
using System.Collections;

public class ProjectileGun : BaseGun
{
    public GameObject Bullet;

    public float MuzzleVelocity;

    public override bool Fire(TargetFunc Target, LayerMask TargetLayer, float Varience = 0, bool DebugDraw = false, bool InGameDraw = false)
    {
        if (!Reloading)
        {
            if (!OnCooldown)
            {
                if (Magazine > 0)
                {
                    if (this.transform.parent.name == "GunHolder")
                    {
                        gunAudioSpawn();
                    }
                    //can fire
                    Vector3 bulletEndDestination = Target() + (Varience * Random.insideUnitSphere);

                    GameObject nProj = Instantiate(Bullet);
                    nProj.transform.position = Muzzle.position;
                    nProj.transform.LookAt(bulletEndDestination);

                    nProj.GetComponent<Projectile>().StartProjectile(MuzzleVelocity, Damage, HeadshotDamage);

                    FiredGun(nProj);
                    return true;
                }
                else
                {
                    Reload();
                }
            }
        }
        return false;
    }

}