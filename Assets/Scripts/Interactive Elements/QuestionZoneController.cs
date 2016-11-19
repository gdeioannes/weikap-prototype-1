using UnityEngine;
using System.Collections;
using DG.Tweening;

public class QuestionZoneController : BaseInteractiveElement {

    [SerializeField] int questionId;
    CharacterControl character;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        this.character = character;
        GameController.Instance.DisplayQuestion(questionId, OnQuestionAnswered);
    }    

    void OnQuestionAnswered(bool result)
    {
        if (result)
        {
            StartCoroutine(MoveQuestionToUI(character));
        }
        else
        {
            Object.Destroy(this.gameObject); // just remove question
        }
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

        character.UpdateQuestion(1);

        Object.Destroy(this.gameObject);

        yield break;
    }
}
