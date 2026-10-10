using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem
{
    Vector2 lastPosition = Vector2.positiveInfinity;
    Vector2 velocity;

    /// <summary>
    ///     Calculates direction for weapon to shoot at target.
    /// </summary>
    /// <param name="dt">Time step at which it updates.</param>
    /// <param name="currentPosition">Current position of a target.</param>
    /// <param name="weapon">Weapon system that has transform component and ProjectileSpeed property.</param>
    /// <returns>Returns future position of a target.</returns>
    virtual public Vector2 UpdateTargetPosition(float dt, Vector2 currentPosition, Weapon weapon)
    {
        if (lastPosition == Vector2.positiveInfinity)
            lastPosition = currentPosition;
        velocity = (currentPosition - lastPosition) / dt;
        lastPosition = currentPosition;

        Vector2 direction = currentPosition - (Vector2)weapon.transform.position;

        Vector2 finalPosition = currentPosition + velocity / weapon.ProjectileSpeed * direction.magnitude;

        Debug.DrawLine(weapon.transform.position, finalPosition, Color.yellow);

        return finalPosition;
    }
}
