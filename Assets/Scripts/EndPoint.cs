using UnityEngine;
using System.Collections;

public class EndPoint : BaseInteractiveElement
{
   protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnCharacterEnter(CharacterControl character)
    {
		GameController.Instance.UpdateLevelProgress(PlayerData.LevelStatus.Win);        
    }
}
