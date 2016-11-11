using UnityEngine;
using System.Collections;

public class ConsumableController : BaseInteractiveElement
{
    public enum ConsumableType
    {
        Coin,
        Sample
    }

    public ConsumableType type;
    [SerializeField] int amount = 1;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        character.UpdateConsumable(type, amount);
        base.OnCharacterEnter(character);
    }
}
