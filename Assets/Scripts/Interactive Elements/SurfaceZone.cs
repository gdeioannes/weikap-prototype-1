using UnityEngine;
using System.Collections;

public class SurfaceZone : BaseInteractiveElement {

    protected override void OnCharacterEnter(CharacterControl character)
    {
        character.IsOnSurface = true;
    }

    protected override void OnCharacterExit(CharacterControl character)
    {
        character.IsOnSurface = false;
    }
}
