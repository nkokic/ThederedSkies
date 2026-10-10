using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    float lifeTime;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}
