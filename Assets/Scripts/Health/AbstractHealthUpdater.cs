using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractHealthUpdater : MonoBehaviour, IObserver<Hitable>
{
    [SerializeField]
    protected Health health;

    protected virtual void Awake()
    {
        if (health)
            health.AddObserver(this);
    }

    public virtual void StartObserver(Hitable reference)
    {
        health = (Health)reference;
        UpdateObserver();
    }
    public virtual void UpdateObserver()
    {
        Debug.LogError(GetType().Name + " has not implemented UpdateObserver()");
    }

    public virtual void StopObserver()
    {
        Debug.LogWarning(GetType().Name + " has not implemented StopObserver()");
    }
}
