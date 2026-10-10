using UnityEngine;
public abstract class CharacterBehaviour
{
    protected Character character;

    const bool usesDirectMovement = true;

    public CharacterBehaviour() { }

    public CharacterBehaviour(Character character)
    {
        this.character = character;
    }

    virtual public void Start(Character character)
    {
        this.character = character;
        character.GetVehicle().SetDirectMovement(!usesDirectMovement);
        Debug.Log($"{character.name} changed state to {GetType().Name}");
    }

    abstract public void Update();

    virtual public void End()
    {
        Debug.Log($"{GetType().Name} state ended in {character.name}");
    }

    protected void ChangeState(CharacterBehaviour behaviour)
    {
        character.SetBehaviour(behaviour);
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
    }

    abstract public CharacterBehaviour Clone();
}



