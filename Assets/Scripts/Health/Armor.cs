using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor
{
    [SerializeField]
    private float maxArmor = 100;

    [SerializeField]
    private float[] armorPlates;

    public float GetMaxArmor => maxArmor;

    public void AddArmorRingValues(float value)
    {
        for (int i = 0; i < armorPlates.Length; i++)
        {
            AddArmorRingValue(value, i);
        }
    }

    public void AddArmorRingValue(float value, int i)
    {
        armorPlates[i] = Mathf.Clamp(armorPlates[i] + value, 0, maxArmor);
    }

    public virtual float CalculateArmorDamage(Vector2 direction, float damage)
    {
        GetArmorValues(direction, out int lowerPlate, out int upperPlate, out float lowerBias, out float upperBias);

        armorPlates[lowerPlate] -= damage * lowerBias;
        armorPlates[upperPlate] -= damage * upperBias;

        damage = 0;
        if (armorPlates[lowerPlate] < 0)
        {
            damage += -armorPlates[lowerPlate];
            armorPlates[lowerPlate] = 0;
        }
        if (armorPlates[upperPlate] < 0)
        {
            damage += -armorPlates[upperPlate];
            armorPlates[upperPlate] = 0;
        }
        return damage;
    }

    //direction is relative to object position
    public float GetArmorValues(Vector2 direction, out int lowerPlate, out int upperPlate, out float lowerBias, out float upperBias)
    {
        float hitAngle = (Vector2.SignedAngle(Vector2.up, direction) / 360 + 1) * armorPlates.Length % armorPlates.Length;
        lowerPlate = Mathf.FloorToInt(hitAngle);
        lowerBias = hitAngle - lowerPlate;

        upperPlate = (lowerPlate + 1) % armorPlates.Length;
        upperBias = 1 - lowerBias;

        return armorPlates[lowerPlate] * lowerBias + armorPlates[upperPlate] * upperBias;
    }

    public float GetArmorValues(Vector2 direction)
    {
        return GetArmorValues(direction, out _, out _, out _, out _);
    }

    public IList<float> GetArmorValues()
    {
        return armorPlates;
    }

    public float GetArmorFactor(Vector2 direction)
    {
        return GetArmorValues(direction, out _, out _, out _, out _) / GetMaxArmor;
    }
}
