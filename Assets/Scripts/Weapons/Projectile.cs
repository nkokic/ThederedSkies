using System.Collections.Generic;
using UnityEngine;

public struct Projectile
{
    public static string[] targetableLayer = { "AirObject", "GroundObject" };
    public static float recomendedMinimumSpeed = 30;

    public Vector2 direction;
    public Vector2 position;

    public float remainingRange;
    public float speed;

    public float damage;
    public float armorPiercing;
    public float electroMagnetism;

    public Projectile(float damage, float armorPiercing, float electroMagnetism, 
                        Vector2 direction, Vector2 position, float remainingRange, float speed)
    {
        this.damage = damage;
        this.armorPiercing = armorPiercing;
        this.electroMagnetism = electroMagnetism;
        this.direction = direction;
        this.position = position;
        this.remainingRange = remainingRange;
        this.speed = speed;
        if (speed < recomendedMinimumSpeed)
            Debug.LogWarning($"Projectile speed is lower than recommended minimum speed ({recomendedMinimumSpeed})");
    }

    public void Ricochet(Vector2 normal, float deflection)
    {
        Vector2 reflectedVector = Vector2.Reflect(direction, normal);
        direction = Vector3.Slerp(direction, reflectedVector, deflection).normalized;
        remainingRange *= armorPiercing;
    }

    public Projectile(float damage, Vector2 direction, Vector2 position, float remainingRange, float speed)
        : this (damage, 1, 1, direction, position, remainingRange, speed)
    { }
}
