﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ConsumableController : BaseInteractiveElement
{
    public enum ConsumableType
    {
        Coin,
        Sample
    }

    public ConsumableType type;
    public int id;
    public int amount = 1;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        StartCoroutine(IncrementConsumableCountCoroutine(character));
    }

    IEnumerator IncrementConsumableCountCoroutine(CharacterControl character)
    {
        Vector3 worldPosition = transform.position;
        Vector3 screenPosition = Utils.WorldToCanvasPosition(worldPosition, GameController.Instance.WorldCamera, GameController.Instance.UIMainRectTransform);
        screenPosition.z = 0;
                
        transform.SetParent(GameController.Instance.UIMainRectTransform, true);
        gameObject.layer = LayerMask.NameToLayer("UI");
        transform.localPosition = screenPosition;

        Vector3 endPosition = Vector3.zero;
        switch (type)
        {
            case ConsumableType.Coin:
                endPosition = GameController.Instance.CoinsIconContainer.position;
                break;
            case ConsumableType.Sample:
                endPosition = GameController.Instance.GetSampleIconContainer(id).position;
                break;
        }

        var tweener = transform.DOMove(endPosition, GameController.Instance.consumablesMoveToUITime);
        tweener.SetEase(GameController.Instance.consumableMovEaseType);
        yield return tweener.WaitForCompletion();
        yield return null;
        GameController.Instance.UpdateConsumable(type, id, amount);
        base.OnCharacterEnter(character);

        yield break;
    }
}
