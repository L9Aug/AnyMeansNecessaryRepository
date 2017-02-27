using UnityEngine;
using System.Collections;

public class DistractionGun : BaseGun
{

    public GameObject Projectile;
    public float MuzleVelocity;

    /// <summary>
    /// Fires this gun if the gun is able to fire.
    /// </summary>
    /// <param name="Target">A Fucntion that returns the target you would like to shoot at (This way it only calcs the target if the gun will fire).</param>
    /// <param name="TargetLayer">The layer of the target you wish to hit.</param>
    /// <param name="Varience">The amount that the gun can miss by (in unity units).</param>
    /// <param name="DebugDraw">Should the bullet path be drawn using debug draws?</param>
    /// <param name="InGameDraw">Should the bullet path be drawn using in game visulisation?</param>
    /// <returns>Returns True if the gun fired successfully.</returns>
    public override bool Fire(TargetFunc Target, LayerMask TargetLayer, float Varience = 0, bool DebugDraw = false, bool InGameDraw = false)
    {
        if (!Reloading)
        {
            if (!OnCooldown)
            {
                if (Magazine > 0)
                {
                    // can fire
                    GameObject nProjectile = Instantiate(Projectile);
                    nProjectile.transform.position = Muzzle.position;
                    nProjectile.transform.forward = Muzzle.forward;
                    nProjectile.GetComponent<Rigidbody>().AddForce((Target() - Muzzle.position) * MuzleVelocity, ForceMode.VelocityChange);

                    FiredGun(nProjectile);

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
