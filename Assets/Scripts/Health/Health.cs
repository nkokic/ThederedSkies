using System.Collections;
using UnityEngine;

public class Health : Hitable
{
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float health;

    public float GetHealth => health;
    public float GetMaxHealth => maxHealth;

    [SerializeField]
    public Armor armor;

    bool isDying = false;

    const float crashThreshold = 3;
    const float crashMultiplier = 5;

    const float grindThreshold = 1;
    const float grindMultiplier = 5;

    const float crushThreshold = 0f;
    const float crushMultiplier = 1f;

    [SerializeField]
    ParticleSystem deathParticles;



    void Start()
    {
        if (!rigidBody.GetComponent<Collider2D>())
            Debug.LogWarning(name + " does not have a collider");
    }

    void SubtractHealth(float damage)
    {
        Notify();

        if (isDying)
            return;

        health -= Mathf.Abs(damage);

        if (health <= 0)
            StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        isDying = true;
        deathParticles = Instantiate(deathParticles, transform);
        deathParticles.Play();
        yield return new WaitForSeconds(2);

        deathParticles.Stop();
        deathParticles.transform.parent = null;
        ClearObservers();
        Destroy(gameObject);
    }

    public void HitObject(Vector2 point, Vector2 normal, Projectile projectile)
    {
        float dotProduct = Vector2.Dot(-normal, projectile.direction);
        Vector2 relativeHitPoint = point - rigidBody.position;

        TakeKick(-normal * dotProduct * projectile.damage, point);
        SubtractHealth(armor.CalculateArmorDamage(relativeHitPoint, projectile.damage * dotProduct));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 contactDirection = contact.point - rigidBody.position;
        float armor = this.armor.GetArmorValues(contactDirection) / this.armor.GetMaxArmor;

        float collisionDamage = GetGrindDamage(contact, armor) + GetCrushDamage(contact, armor);

        SubtractHealth(this.armor.CalculateArmorDamage(contactDirection, collisionDamage));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 contactDirection = contact.point - rigidBody.position;
        float armor = this.armor.GetArmorValues(contactDirection) / this.armor.GetMaxArmor;

        float collisionDamage = GetCrashDamage(contact, armor);

        SubtractHealth(this.armor.CalculateArmorDamage(contactDirection, collisionDamage));
    }

    float GetCrashDamage(ContactPoint2D contact, float armor) => MathExpanded.Ramp(contact.relativeVelocity.magnitude, crashThreshold) * crashMultiplier;
    float GetCrushDamage(ContactPoint2D contact, float armor) => MathExpanded.Ramp(Mathf.Abs(contact.normalImpulse), crushThreshold) * crushMultiplier;
    float GetGrindDamage(ContactPoint2D contact, float armor) => MathExpanded.Ramp(Mathf.Abs(contact.tangentImpulse), grindThreshold) * grindMultiplier;

    public void AddHealth(float healling)
    {
        health = Mathf.Clamp(Mathf.Abs(healling), -maxHealth, maxHealth);
        Notify();
    }

    void TakeKick(Vector2 force, Vector2 position)
    {
        if (rigidBody)
            rigidBody.AddForceAtPosition(force, position);
    }

    
}
