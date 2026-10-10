using UnityEngine;

class Idle : CharacterBehaviour
{
    public Idle(Character character) : base(character) { }

    public Idle() { }

    public override void Update()
    {
        character.GetVehicle().SetMovementVector(character.transform.position);
    }

    public override CharacterBehaviour Clone()
    {
        return new Idle(character);
    }
}

