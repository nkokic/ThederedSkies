using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractMovementUpdater : MonoBehaviour, IObserver<Movement>
{
    [SerializeField]
    Vehicle vehicle;

    protected Movement movement;

    [SerializeField]
    protected Vector2 vectorMultiplier;
    [SerializeField]
    protected Vector2 offset;

    protected virtual void Awake()
    {
        if (vehicle)
            vehicle.Movement.AddObserver(this);
    }

    public virtual void StartObserver(Movement reference)
    {
        movement = reference;
        UpdateObserver();
    }

    protected float CalculateEngineThrust()
    {
        return (movement.Thrust / movement.MaxThrust * vectorMultiplier + offset).magnitude;
    }

    public virtual void StopObserver()
    {
        Debug.LogWarning(GetType().Name + " has not implemented StopObserver()");
    }

    public virtual void UpdateObserver()
    {
        Debug.LogError(GetType().Name + " has not implemented UpdateObserver()");
    }
}
