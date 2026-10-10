using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : IObservable<Movement>
{
    //movement
    protected Vector2 targetPosition = Vector2.zero, targetRotation, thrust, torque;

    protected Rigidbody2D rigidbody;

    [SerializeField]
    protected float maxSpeed;

    [SerializeField]
    protected float angularSpeed;

    private float speedLast = 0;
    protected float acceleration;

    protected Action<Vector2> moveAction; //switch delegate between direct and world point movement
    protected Action rotationAction;

    private List<IObserver<Movement>> observers = new List<IObserver<Movement>>();

    public Vector2 Thrust => thrust;
    public virtual float MaxThrust => maxSpeed;

    // Start is called before the first frame update
    public virtual void Start(Rigidbody2D rigidbody)
    {
        this.rigidbody = rigidbody;
        moveAction = Move;
        rotationAction = RotateAuto;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CalculateAcceleration();

        moveAction(targetPosition);
        rotationAction();

        Notify();
    }

    protected float CalculateAcceleration()
    {
        acceleration = (rigidbody.velocity.magnitude - speedLast) / Time.fixedDeltaTime;
        speedLast = rigidbody.velocity.magnitude;
        return acceleration;
    }

    protected virtual void Move(Vector2 targetDirection)
    {
        thrust = Vector2.ClampMagnitude(targetDirection * maxSpeed, maxSpeed);

        AddThrust(thrust);
    }

    protected virtual void MoveTo(Vector2 targetPosition)
    {
        Vector2 error = targetPosition - rigidbody.position;
        Vector2 snapVelocity = error / Time.fixedDeltaTime;
        thrust = Vector2.ClampMagnitude(snapVelocity, maxSpeed);

        AddThrust(thrust);
    }

    public virtual void AddThrust(Vector2 thrust)
    {
        rigidbody.velocity = thrust;
    }

    public void SwitchToDirectMovement(bool check)
    {
        if (check)
            moveAction = Move;
        else
            moveAction = MoveTo;
    }

    public void SwitchToDirectRotation(bool check)
    {
        if (check)
            rotationAction = Rotate;
        else
            rotationAction = RotateAuto;
    }

    virtual protected void RotateAuto()
    {
        if (rigidbody.velocity.magnitude > 0.25f)
            return;

        float targetAngle = Vector2.SignedAngle(Vector2.up, rigidbody.velocity.normalized);
        rigidbody.SetRotation(Mathf.MoveTowardsAngle(rigidbody.rotation, targetAngle, angularSpeed * Time.fixedDeltaTime));
    }

    protected void Rotate()
    {
        float targetAngle = Vector2.SignedAngle(Vector2.up, targetRotation);
        rigidbody.SetRotation(Mathf.MoveTowardsAngle(rigidbody.rotation, targetAngle, angularSpeed * Time.fixedDeltaTime));
    }

    public void SetMovementVectors(Vector2 targetPosition, Vector2 targetRotation)
    {
        this.targetPosition = targetPosition;
        this.targetRotation = targetRotation;
    }

    public void SetMovementVector(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public Vector2 GetPosition()
    {
        return rigidbody.position;
    }
    public Vector2 GetVelocity()
    {
        return rigidbody.velocity;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void SetRigidbody(Rigidbody2D rigidbody)
    {
        this.rigidbody = rigidbody;
    }


    void Notify()
    {
        if (observers.Count < 1)
            return;
        foreach (var observer in observers)
            observer.UpdateObserver();
    }

    void ClearObservers()
    {
        if (observers.Count < 1)
            return;
        foreach (var observer in observers)
            observer.StopObserver();
        observers.Clear();
    }

    public void AddObserver(IObserver<Movement> observer)
    {
        observers.Add(observer);
        observer.StartObserver(this);
    }

    public void RemoveObserver(IObserver<Movement> observer)
    {
        observer.StopObserver();
        observers.Remove(observer);
    }
}
