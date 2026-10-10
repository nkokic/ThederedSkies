using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBehaviour
{
    const bool usesDirectMovement = true;

    public Player(Character character) : base (character)
    {
        MainCameraObject.SetTargetObject(character.transform);
        character.GetVehicle().SetDirectMovement(true);
    }

    public Player() { }

    public override void Start(Character character)
    {
        this.character = character;
        character.GetVehicle().SetDirectMovement(usesDirectMovement);
        MainCameraObject.SetTargetObject(character.transform);
        Debug.Log($"{character.name} changed state to {GetType().Name}");
    }
    public override void Update()
    {
        Vector2 vector = new Vector2(
                                    Input.GetAxis("Horizontal"),
                                    Input.GetAxis("Vertical"));

        character.GetVehicle().SetMovementVector(vector);
        character.GetVehicle().GetWeapon().TurretDriver.LookAt(
                                                    Camera.main.ScreenToWorldPoint(
                                                    Input.mousePosition)
                                                );

        if (Input.GetButton("Fire1"))
        {
            Weapon weapon = character.GetVehicle().GetWeapon();
            weapon.Trigger();
            MainCameraObject.AddShake(weapon.Damage / 1000f); // TODO: make sense of shaking numbers
        }
    }

    public override CharacterBehaviour Clone()
    {
        return new Player(character);
    }
}
