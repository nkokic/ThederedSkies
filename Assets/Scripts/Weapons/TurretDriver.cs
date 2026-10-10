using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretDriver
{
    [SerializeField]
    float angularSpeed = 60;

    [SerializeField]
    Transform transform;

    readonly float averageTargetRadius = 1;

    Quaternion targetAngle;

    public void LookAt(Vector2 position)
    {
        float angle = MathExpanded.CalculateAngleFromLine(transform.position, position);
        targetAngle = Quaternion.Euler(0, 0, angle);
    }

    public bool IsInAngleRange(Vector2 position)
    {
        float distance = (position - (Vector2)transform.position).magnitude;
        float deltaAngle = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetAngle.eulerAngles.z);

        return Mathf.Abs(deltaAngle) < averageTargetRadius / distance;
    }

    public void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, angularSpeed * Time.deltaTime);
    }

    public Vector2 GetDirection()
    {
        float angle = (transform.eulerAngles.z - 90) * Mathf.Deg2Rad;
        return MathExpanded.Angle2Direction(angle);
    }
}
