using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    float damage, range, projectileSpeed = float.PositiveInfinity, spread, roundsPerMinute, armorPiercing, electroMagnetism;
    
    [SerializeField]
    int multishot = 1;

    Vehicle owner = null;

    float ShootPeriod => 60 / roundsPerMinute;
    float ProjectileLifeTime => range / projectileSpeed;
    public float RoundsPerMinute
    {
        get => roundsPerMinute;
        set
        {
            roundsPerMinute = value;
            CalculatePoolSize();
        }
    }
    public float Damage { get => damage; set => damage = value; }
    public float Range { 
        get => range;
        set
        {
            range = value;
            CalculatePoolSize();
        }
    }
    public float ProjectileSpeed {
        get
        {
            return Mathf.Min(projectileSpeed, Range / Time.fixedDeltaTime);
        }
        set 
        { 
            projectileSpeed = value;
            CalculatePoolSize();
        } 
    }
    public float Spread { get => spread; set => spread = value; }
    public int Multishot 
    { 
        get => multishot;
        set
        { 
            multishot = value;
            CalculatePoolSize();
        }
    }

    [SerializeField]
    Transform barrelTransform;

    [SerializeField]
    AudioSource audioSource;

    private float soundPitchDispersion = 0.05f;

    [SerializeField]
    Bullet projectile;

    [SerializeField]
    TurretDriver turretDriver;
    public TurretDriver TurretDriver { get => turretDriver; private set => turretDriver = value; }

    Pool<Bullet> projectiles = null;
    
    GameObject projectilePool;

    TargetingSystem targetingSystem;


    private void Awake()
    {
        owner = GetComponentInParent<Vehicle>();
        if (!owner)
            print(name + " is orphaned weapon.");
        else
            print(name + " is owned by " + owner.name);

        targetingSystem = new PreciseTargetingSystem();
        projectilePool = new GameObject(name + " projectile pool");
        CalculatePoolSize();
    }

    private void Update()
    {
        turretDriver.Update();
    }

    void CalculatePoolSize()
    {
        int capacity = Mathf.CeilToInt(range / ProjectileSpeed / ShootPeriod * multishot + 1);
        if (projectiles != null)
        {
            projectiles.SetCapacity(capacity);
        }
        else
        {
            projectiles = new Pool<Bullet>(capacity, projectile, ProjectileLifeTime, projectilePool.transform);
        }
    }

    bool isShooting = false;
    public void Trigger()
    {
        if(!isShooting)
            StartCoroutine(Shoot());
    }

    public bool IsInRange(Vector2 position)
    {
        return (position - (Vector2)barrelTransform.position).magnitude < range;
    }

    public void UseTargetingSystem(Vector2 position)
    {
        turretDriver.LookAt(targetingSystem.UpdateTargetPosition(Time.deltaTime, position, this));
    }

    void ShootProjectile()
    {
        Bullet projectile = projectiles.GetObject();
        projectile.transform.position = barrelTransform.transform.position;

        Vector2 direction = Quaternion.AngleAxis(Random.Range(-spread / 2, spread / 2), Vector3.forward) * barrelTransform.up;
        projectile.SetProjectile(new Projectile(damage, armorPiercing, electroMagnetism,
                                                direction, barrelTransform.position, range, ProjectileSpeed));
        SendRecoil(direction);
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        audioSource.pitch = Random.Range(1 - soundPitchDispersion, 1 + soundPitchDispersion);

        audioSource.PlayOneShot(audioSource.clip);

        for (int i = 0; i < multishot; i++)
            ShootProjectile();

        yield return new WaitForSeconds(ShootPeriod);
        isShooting = false;
    }

    public void SwitchWith(Weapon weapon, bool destroyOld = false)
    {
        Transform previousParent = weapon.transform.parent;
        Vector3 previousPosition = weapon.transform.position;
        Vehicle previousOwner = weapon.owner;

        weapon.transform.parent = this.transform.parent;
        weapon.transform.position = this.transform.position;
        weapon.owner = this.owner;

        this.transform.parent = previousParent;
        this.transform.position = previousPosition;
        this.owner = previousOwner;

        if (previousOwner)
            previousOwner.SetWeapon(this);
        weapon.owner.SetWeapon(weapon);

        if (destroyOld)
            Destroy(gameObject);
    }

    void SendRecoil(Vector2 direction)
    {
        Vector2 recoil = direction * damage;
    }

    private void OnDestroy()
    {
        Destroy(projectilePool, ProjectileLifeTime);
    }
}
