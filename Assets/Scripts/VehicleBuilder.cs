using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBuilder : MonoBehaviour
{
    [SerializeField]
    string codeName = "";

    int codeNumber = 1;

    private readonly bool destroyOldWeapon = true;

    [SerializeField]
    private Vehicle vehicleType;

    [SerializeReference, SubclassPicker]
    private CharacterBehaviour behaviour;

    [SerializeField]
    Weapon weaponType;

    public void BuildVehicle()
    {
        Vehicle vehicle = Instantiate(vehicleType, transform.position, Quaternion.identity);
        Character pilot = vehicle.gameObject.AddComponent<Character>();
        vehicle.name = codeName + "-" + codeNumber;
        codeNumber++;

        vehicle.SetPilot(pilot);
        pilot.SetBehaviour(behaviour.Clone());
        
        Weapon weapon = Instantiate(weaponType, vehicle.transform);
        vehicle.GetWeapon().SwitchWith(weapon, destroyOldWeapon);
    }
}
