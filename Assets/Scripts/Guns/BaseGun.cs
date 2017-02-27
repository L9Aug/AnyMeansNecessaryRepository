using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioReverbZone))]
public class BaseGun : MonoBehaviour {

    // the muzzle of the gun.
    public Transform Muzzle;
    // the number of bullets the gun can store.
    public int BaseMagazineSize;
    public int MagazineSize;
    // the time it takes this gun to reload.
    public float ReloadTime;
    // the number of bullets this gun can fire per second.
    public float RateofFire;
    // the damage of each bullet if it hits a target.
    public float BaseDamage;
    public float Damage;
    // the range of the gun.
    public float Range = 1000f;
    // does this gun have infinite Ammo.
    public bool InfiniteAmmo = true;
    // How much ammo does this gun have in reserve.
    public int AmmoReserve;
    // Is this gun silenced.
    public bool isSilenced = false;

    public List<AudioClip> FiringSounds = new List<AudioClip>();

    public List<AudioClip> ReloadingSounds = new List<AudioClip>();

    [Range(0f, 1f)]
    public float animSpeed = 1;

    public Transform RightHand;
    public Transform LeftHand;
    [Range(0f, 1f)]
    public float RightHandPositionWeight = 0;
    [Range(0f, 1f)]
    public float LeftHandPositionWeight = 0;
    [Range(0f, 1f)]
    public float RightHandRotationWeight = 0;
    [Range(0f, 1f)]
    public float LeftHandRotationWeight = 0;

    protected AudioSource audioSource;
    protected float BulletPathDisplayTime = 0.5f;
    protected bool Reloading = false;
    protected float reloadTimer = 0;
    protected bool OnCooldown = false;
    protected float cooldownTimer = 0;
    protected int Magazine;

    public delegate Vector3 TargetFunc();
    public delegate void UpdateWeapon(int magazine, int magazineSize);
    public delegate void ReloadUpdate(int ammoReserve, GameObject Weapon);
    public delegate void OnFireCallback(GameObject Projectile);
    public List<OnFireCallback> OnFireCallbacks = new List<OnFireCallback>();
    public UpdateWeapon updateWeapon;
    public ReloadUpdate reloadUpdate;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        ReloadAction();
        CallUpdateWeapon();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Fires this gun if the gun is able to fire.
    /// </summary>
    /// <param name="Target">A Fucntion that returns the target you would like to shoot at (This way it only calcs the target if the gun will fire).</param>
    /// <param name="TargetLayer">The layer of the target you wish to hit.</param>
    /// <param name="Varience">The amount that the gun can miss by (in unity units).</param>
    /// <param name="DebugDraw">Should the bullet path be drawn using debug draws?</param>
    /// <param name="InGameDraw">Should the bullet path be drawn using in game visulisation?</param>
    /// <returns>Returns True if the gun fired successfully.</returns>
    public virtual bool Fire(TargetFunc Target, LayerMask TargetLayer, float Varience = 0, bool DebugDraw = false, bool InGameDraw = false)
    {
        if (!Reloading)
        {
            if (!OnCooldown)
            {
                if (Magazine > 0)
                {
                    //can fire
                    RaycastHit hit = new RaycastHit();
                    Vector3 bulletEndDestination = Target() + (Varience * Random.insideUnitSphere);
                    Ray BulletPath = new Ray(Muzzle.position, (bulletEndDestination - Muzzle.position).normalized);
                    hit.point = BulletPath.origin + (BulletPath.direction * Range);
                    if (Physics.Raycast(BulletPath, out hit, Range, ~0, QueryTriggerInteraction.Ignore))
                    {
                        hit.collider.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                    }
                    if (DebugDraw) Debug.DrawLine(BulletPath.origin, hit.point, Color.green, 1f);
                    if (InGameDraw) DrawBulletPathInGame(Muzzle, hit.point);
                    FiredGun(null);
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

    /// <summary>
    /// Fires this gun if the gun is able to fire.
    /// </summary>
    /// <param name="Target">The location you want to shoot at.</param>
    /// <param name="TargetLayer">The layer of the target you wish to hit.</param>
    /// <param name="Varience">The amount that the gun can miss by (in unity units).</param>
    /// <param name="DebugDraw">Should the bullet path be drawn using debug draws?</param>
    /// <param name="InGameDraw">Should the bullet path be drawn using in game visulisation?</param>
    /// <returns>Returns True if the gun fired successfully.</returns>
    public virtual bool Fire(Vector3 Target, LayerMask TargetLayer, float Varience = 0, bool DebugDraw = false, bool InGameDraw = false)
    {
        return Fire(() => { return Target; }, TargetLayer, Varience, DebugDraw, InGameDraw);
    }


    protected void CallOnFireCallbacks(GameObject projectile)
    {
        foreach(OnFireCallback callback in OnFireCallbacks)
        {
            if(callback != null)
            {
                callback(projectile);
            }
        }
    }

    protected void FiredGun(GameObject projectile)
    {
        OnCooldown = true;
        --Magazine;

        CallOnFireCallbacks(projectile);

        if(FiringSounds.Count > 0)
        {
            audioSource.PlayOneShot(FiringSounds[Random.Range(0, FiringSounds.Count)]);
        }

        CallUpdateWeapon();
        StartCoroutine(CooldownTick());
    }

    void DrawBulletPathInGame(Transform StartPos, Vector3 EndPos)
    {
        BulletTrail bt = Instantiate<BulletTrail>(Resources.Load<BulletTrail>("BulletTrail"));
        bt.SetupBulletTrial(StartPos, EndPos, BulletPathDisplayTime);
    }

    public void CallUpdateWeapon()
    {
        if (updateWeapon != null)
        {
            if (InfiniteAmmo)
            {
                updateWeapon(Magazine, MagazineSize);
            }
            else
            {
                updateWeapon(Magazine, AmmoReserve);
            }
        }
    }

    public virtual void Reload()
    {
        if (!Reloading)
        {
            Reloading = true;
            if(ReloadingSounds.Count > 0)
            {
                audioSource.PlayOneShot(ReloadingSounds[Random.Range(0, ReloadingSounds.Count)]);
            }
            StartCoroutine(ReloadTick());
        }
    }

    /// <summary>
    /// CooldownTick happens at the end of each frame that the gun is cooling down after fireing.
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownTick()
    {
        while (OnCooldown)
        {
            yield return null;
            cooldownTimer += Time.deltaTime;
            if(cooldownTimer >= (1f / RateofFire))
            {
                OnCooldown = false;
                cooldownTimer = 0;
            }
        }
    }

    /// <summary>
    /// ReloadTick happens at the end of each frame whilst reloading.
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadTick()
    {
        while (Reloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= ReloadTime)
            {
                ReloadAction();
            }
            yield return null;
        }
    }

    protected virtual void ReloadAction()
    {
        Reloading = false;
        reloadTimer = 0;

        if (InfiniteAmmo)
        {
            Magazine = MagazineSize;
        }
        else
        {
            AmmoReserve += Magazine;

            if(AmmoReserve < MagazineSize)
            {
                Magazine = AmmoReserve;
                AmmoReserve = 0;
            }
            else
            {
                Magazine = MagazineSize;
                AmmoReserve -= MagazineSize;
            }

            if(reloadUpdate != null)
            {
                reloadUpdate(AmmoReserve, gameObject);
            }
        }

        CallUpdateWeapon();
    }

    void OnDestroy()
    {
        AmmoReserve += Magazine;
        if (reloadUpdate != null)
        {
            reloadUpdate(AmmoReserve, gameObject);
        }
    }

    void OnEnable()
    {
        if (Reloading)
        {
            StartCoroutine(ReloadTick());
        }
        if (OnCooldown)
        {
            StartCoroutine(CooldownTick());
        }
    }

}
