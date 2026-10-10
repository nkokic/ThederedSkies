using UnityEngine;

[System.Serializable]
public class PIDControler : IControler
{
    [SerializeField]
    float proportionalGain = 1;
    [SerializeField]
    float derivativeGain = 0.3f;
    [SerializeField]
    float integralGain = 0;

    [SerializeField]
    Vector2 result = Vector2.zero;

    bool firstStart = true;
    Vector2 lastError;

    [SerializeField]
    private Vector2 I;

    public Vector2 Update(float dt, Vector2 error, Rigidbody2D rigidbody, float outputMax, float velocityMax)
    {
        if (dt <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(dt));

        Vector2 P = proportionalGain * error;
        PreventIntegralSaturation(dt, error, outputMax);
        Vector2 D = derivativeGain * -rigidbody.velocity;
        result = P + I + D;
        return result;
    }

    public Vector2 Update(float dt, Vector2 error)
    {
        if (dt <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(dt));

        if (firstStart)
        {
            lastError = error;
            firstStart = false;
        }


        Vector2 velocity = (error - lastError) / dt;
        
        lastError = error;

        Vector2 P = proportionalGain * error;

        Vector2 D = derivativeGain * velocity;

        I += error * integralGain * dt;

        result = P + I + D;

        return result;
    }

    private void PreventIntegralSaturation(float dt, Vector2 error, float outputMax)
    {
        Vector2 S = Vector2.one;
        if ((result.x > outputMax && error.x > 0) || (result.x < -outputMax && error.x < 0))
            I.x = 0;
        if ((result.y > outputMax && error.y > 0) || (result.y < -outputMax && error.y < 0))
            I.y = 0;
        I += S * error * integralGain * dt;
    }
}
