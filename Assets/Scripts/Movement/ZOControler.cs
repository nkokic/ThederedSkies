using UnityEngine;

[System.Serializable]
public class ZOControler : IControler
{
    [SerializeField]
    float zeta = .7f;
    [SerializeField]
    float omega = 1f;

    [SerializeField]
    Vector2 result = Vector2.zero;

    [SerializeField]
    private Vector2 I;

    public Vector2 Update(float dt, Vector2 error, Rigidbody2D rigidbody, float outputMax, float velocityMax)
    {
        if (dt <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(dt));
        
        Vector2 P = (omega * omega + 2* zeta * omega * outputMax / velocityMax) * error;
        PreventIntegralSaturation(dt, error, outputMax, velocityMax);
        Vector2 D = 2 * zeta * omega * -rigidbody.velocity;
        result = P + I + D;

        return result;
    }

    private void PreventIntegralSaturation(float dt, Vector2 error, float outputMax, float velocityMax)
    {
        Vector2 S = Vector2.one;
        if ((result.x > outputMax && error.x > 0) || (result.x < -outputMax && error.x < 0))
            S.x = 0;
        if ((result.y > outputMax && error.y > 0) || (result.y < -outputMax && error.y < 0))
            S.y = 0;
        I += S * (omega * omega * outputMax / velocityMax) * error * dt;
    }
}
