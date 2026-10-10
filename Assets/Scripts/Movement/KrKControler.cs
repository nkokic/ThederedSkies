using UnityEngine;

[System.Serializable]
public class KrKControler : IControler
{
    [SerializeField]
    float zeta = .7f;
    [SerializeField]
    float omega = 1f;

    [SerializeField]
    float Kr1 = .7f;
    [SerializeField]
    float Kr2 = 1f;

    [SerializeField]
    Vector2 result = Vector2.zero;

    [SerializeField]
    private Vector2 I;

    [SerializeField]
    Vector2 vTarget;

    public Vector2 Update(float dt, Vector2 error, Rigidbody2D rigidbody, float outputMax, float velocityMax)
    {
        if (dt <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(dt));

        float d = Kr1 / rigidbody.mass;

        float p = 2 * zeta * omega;
        Kr2 = omega * omega / d;

        float a = Mathf.Exp(-p * dt);

        vTarget = Kr2 * (d * error - rigidbody.velocity) + a * vTarget;

        vTarget = Vector2.ClampMagnitude(vTarget, velocityMax);

        Vector2 vError = vTarget - rigidbody.velocity;
        Vector2 P1 = Kr1 * vError;
        PreventIntegralSaturation(dt, vError, rigidbody.mass, outputMax, velocityMax);

        result = P1 + I;

        return result;
    }

    private void PreventIntegralSaturation(float dt, Vector2 error, float mass, float outputMax, float velocityMax)
    {
        Vector2 S = Vector2.one;
        if ((result.x > outputMax && error.x > 0) || (result.x < -outputMax && error.x < 0))
            S.x = 0;
        if ((result.y > outputMax && error.y > 0) || (result.y < -outputMax && error.y < 0))
            S.y = 0;
        I += S * Kr1 * error * dt * outputMax / velocityMax / mass;
    }
}
