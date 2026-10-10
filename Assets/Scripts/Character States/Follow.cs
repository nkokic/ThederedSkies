using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : CharacterBehaviour
{
    [SerializeField]
    float distance;
    [SerializeField]
    protected Transform target;

    public Follow(Character character, Transform target, float distance) : base(character) 
    {
        this.distance = distance;
        this.target = target;
    }

    public Follow() { }
    public override void Update()
    {
        if (!target)
        {
            character.SetBehaviour(new Idle());
            return;
        }

        Vector2 displacement = -(target.position - character.transform.position).normalized * distance;

        character.GetVehicle().SetMovementVector((Vector2)target.position + displacement);
    }

    public override CharacterBehaviour Clone()
    {
        return new Follow(character, target, distance);
    }
}
