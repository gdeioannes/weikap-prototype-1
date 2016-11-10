using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseInteractiveElement : MonoBehaviour
{
    Dictionary<Collider2D, CharacterControl> activeElements;

    void Awake()
    {
        activeElements = new Dictionary<Collider2D, CharacterControl>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        CharacterControl character = collider.GetComponent<CharacterControl>();
        if (character != null) {
            OnCharacterEnter(character);
            activeElements[collider] = character;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!activeElements.ContainsKey(collider)) {
            OnTriggerEnter2D(collider);
        }
        else {
            OnCharacterStay(activeElements[collider]);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (activeElements.ContainsKey(collider)) {
            OnCharacterExit(activeElements[collider]);
        }
    }

    protected virtual void OnCharacterEnter(CharacterControl character)
    {
        // Destroy on contact
        Object.Destroy(this.gameObject);
    }

    protected virtual void OnCharacterStay(CharacterControl character) { }

    protected virtual void OnCharacterExit(CharacterControl character) { }
}
