using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : CharacterBehaviour
{
    [SerializeField]
    Transform target;

    public Attack() { }

    public Attack(Character character, Transform target) : base(character)
    {
        this.target = target;
    }

    public override CharacterBehaviour Clone()
    {
        return new Attack(character, target);
    }

    public override void Update()
    {
        if (!target)
        {
            ChangeState(new Idle(character));
            return;
        }

        Vector2 targetPosition = target.transform.position;
        character.GetVehicle().GetWeapon().UseTargetingSystem(targetPosition);

        if (Time.time % 1 < 0.5f &&
            character.GetVehicle().GetWeapon().IsInRange(targetPosition) && 
            character.GetVehicle().GetWeapon().TurretDriver.IsInAngleRange(targetPosition)
            )
            character.GetVehicle().GetWeapon().Trigger();

    }
}
