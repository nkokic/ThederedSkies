using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vehicle : Health
{
    [SerializeReference, SubclassPicker]
    private Movement movement;

    [SerializeField]
    private Weapon weapon;

    //reference to ability class modifying vehicle properties temporatily

    private Character pillot;

    public void SetPilot(Character pillot)
    {
        this.pillot = pillot;
        pillot.SetVehicle(this);
    }
    public Character GetPilot()
    {
        return pillot;
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public Weapon GetWeapon()
    {
        return weapon;
    }

    public Movement Movement => movement;

    //INVENTORY-CARGO scheme
    /*[SerializeField]
    int cargoCapacity;
    private List<T> cargo;
     */

    private void Awake()
    {
        movement.Start(GetRigidBody);
    }

    private void FixedUpdate()
    {
        movement.Update();
    }

    public void SetDirectMovement(bool isDirect)
    {
        movement.SwitchToDirectMovement(isDirect);
    }
    public void SetMovementVector(Vector2 moveVector)
    {
        movement.SetMovementVector(moveVector);
    }


}
