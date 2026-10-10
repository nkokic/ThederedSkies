using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BehaviourTrigger : MonoBehaviour
{
    [SerializeReference, SubclassPicker]
    public CharacterBehaviour behaviour;

    public string behaviourName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character;

        if(collision.TryGetComponent<Character>(out character))
        {
            character.SetBehaviour(behaviour);
        }
    }
}

