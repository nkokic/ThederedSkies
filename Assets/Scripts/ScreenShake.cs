using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScreenShake
{
    [SerializeField]
    float shakeAmount;

    [SerializeField]
    PIDControler PIDControler;

    [SerializeField]
    private Vector2 position;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private Vector2 acceleration;

    public Vector2 State => position;

    public Vector2 UpdateState(float dt)
    {
        acceleration = PIDControler.Update(dt, -position);
        velocity += acceleration;
        position += velocity;
        return position * shakeAmount;
    }

    public void ShakeScreen(Vector2 impulse)
    {
        velocity += impulse;
    }
}
