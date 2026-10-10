using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseTargetingSystem : TargetingSystem
{
    Vector2 lastPosition = Vector2.positiveInfinity;
    Vector2 velocity;
    /// <summary>
    ///     Calculates direction for weapon to shoot at target with precise position prediction.
    /// </summary>
    /// <param name="dt">Time step at which function should update.</param>
    /// <param name="currentPosition">Current position of a target.</param>
    /// <param name="weapon">Weapon system that has transform component.</param>
    /// <returns></returns>
    public override Vector2 UpdateTargetPosition(float dt, Vector2 currentPosition, Weapon weapon)
    {
        if (lastPosition.Equals(Vector2.positiveInfinity))
            lastPosition = currentPosition;
        velocity = (currentPosition - lastPosition) / dt;
        lastPosition = currentPosition;
        
        Vector2 direction = currentPosition - (Vector2)weapon.transform.position;
        float relativeVelocityAngle = Vector2.Angle(direction, velocity) * Mathf.Deg2Rad;
        float speedRatio = velocity.magnitude / weapon.ProjectileSpeed;

        float distanceToTravel1, distanceToTravel2;
        int numberOfSolutions = MathExpanded.SolveQuadraticEquasion(1 - speedRatio * speedRatio,
                                            2 * speedRatio * direction.magnitude * Mathf.Cos(relativeVelocityAngle),
                                            -(direction.magnitude * direction.magnitude),
                                            out distanceToTravel1,
                                            out distanceToTravel2);

        float timeToTravel = Mathf.Max(-distanceToTravel1, -distanceToTravel2) / weapon.ProjectileSpeed;

        Vector2 finalPosition = currentPosition + velocity * timeToTravel;
        Debug.DrawLine(weapon.transform.position, finalPosition);

        return numberOfSolutions > 0 ? finalPosition : currentPosition;
    }
}
