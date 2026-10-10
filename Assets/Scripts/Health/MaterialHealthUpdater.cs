using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHealthUpdater : AbstractHealthUpdater
{
    Texture2D texture;
    Material material;

    public override void StartObserver(Hitable reference)
    {
        health = (Health)reference;
        material = GetComponent<MeshRenderer>().material;

        UpdateObserver();
    }

    public override void UpdateObserver()
    {
        CreateArmorMask(health.armor.GetArmorValues());
    }

    void CreateArmorMask(IList<float> armorPlates)
    {
        texture = new Texture2D(health.armor.GetArmorValues().Count, 1);
        texture.filterMode = FilterMode.Bilinear;

        for (int i = 0; i < armorPlates.Count; i++)
            texture.SetPixel(i, 1, Color.white * armorPlates[i] / health.armor.GetMaxArmor);
        texture.Apply();

        material.SetTexture("_damageMask", texture);
    }
}
