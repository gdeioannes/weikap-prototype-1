using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageZoneController : BaseInteractiveElement {

    [SerializeField] int damageValue;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        character.UpdateEnergy(-damageValue);
    }

    protected override void OnCharacterStay(CharacterControl character)
    {
        character.UpdateEnergy(-damageValue);
    }   
}
