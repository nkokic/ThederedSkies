using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Vehicle vehicle;
    
    [SerializeReference, SubclassPicker]
    private CharacterBehaviour behaviour;

    [SerializeReference]
    private Reputation reputation;

    //readable states: dormant, traveling, managing storage, fighting

    //conditional Identification of other characters

    public void SetBehaviour(CharacterBehaviour behaviour)
    {
        if(this.behaviour != null)
            this.behaviour.End();
        this.behaviour = behaviour;
        behaviour.SetCharacter(this);
        behaviour.Start(this);
    }

    public void SetToFollow(Transform transform)
    {
        SetBehaviour(new Follow(this, transform, 0));
    }

    public void SetToPlayer()
    {
        SetBehaviour(new Player(this));
    }

    public Vehicle GetVehicle()
    {
        return vehicle;
    }

    public void SetVehicle(Vehicle vehicle)
    {
        this.vehicle = vehicle;
    }

    private void Start()
    {
        if (behaviour == null)
            SetBehaviour(new Idle(this));
        behaviour.Start(this);
        if (vehicle)
        {
            vehicle.SetPilot(this);
        }
    }

    private void Update()
    {
        behaviour.Update();
    }
}

