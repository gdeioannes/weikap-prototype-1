using UnityEngine;
using System.Collections;
using DG.Tweening;

public class QuestionZoneController : BaseInteractiveElement {

    [SerializeField] int questionId;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        DisplayQuestion(character);
    }

    void DisplayQuestion(CharacterControl character)
    {
        StartCoroutine(MoveQuestionToUI(character));
    }

    IEnumerator MoveQuestionToUI(CharacterControl character)
    {
        Vector3 worldPosition = transform.position;
        Vector3 screenPosition = Utils.WorldToCanvasPosition(worldPosition, GameController.Instance.WorldCamera, GameController.Instance.UIMainRectTransform);
        screenPosition.z = 0;

        transform.SetParent(GameController.Instance.UIMainRectTransform, true);
        gameObject.layer = LayerMask.NameToLayer("UI");
        transform.localPosition = screenPosition;

        Vector3 endPosition = GameController.Instance.QuestionsIconContainer.position;

        var tweener = transform.DOMove(endPosition, GameController.Instance.consumablesMoveToUITime);
        tweener.SetEase(GameController.Instance.consumableMovEaseType);
        yield return tweener.WaitForCompletion();
        yield return null;

        Object.Destroy(this.gameObject);

        yield break;
    }
}
