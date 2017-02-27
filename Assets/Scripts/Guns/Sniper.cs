using UnityEngine;
using System.Collections;

/// <summary>
/// Tempory class
/// </summary>
public class Sniper : ProjectileGun
{
    public bool isADS = false;

    Transform TempMuzzle;

    public override bool Fire(TargetFunc Target, LayerMask TargetLayer, float Varience = 0, bool DebugDraw = false, bool InGameDraw = false)
    {
        if (isADS)
        {
            TempMuzzle = Muzzle;
            Muzzle = Camera.main.transform;
        }

        bool ReturnVal = base.Fire(Target, TargetLayer, Varience, DebugDraw, InGameDraw);

        if (isADS)
        {
            Muzzle = TempMuzzle;
        }

        return ReturnVal;
    }

}
