using UnityEngine;
public interface IControler
{
    public Vector2 Update(float dt, Vector2 error, Rigidbody2D rigidbody, float outputMax, float velocityMax);
}