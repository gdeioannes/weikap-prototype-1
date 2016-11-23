using UnityEngine;
using System.Collections;
using DG.Tweening;

public class QuestionZoneController : BaseInteractiveElement {

    [SerializeField] int questionId;
    CharacterControl character;
    bool questionDisplayed = false;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        this.character = character;

        if (character.IsInvincible)
        {   // restriction added. unable to interact with questions if invincible
            return;
        }

        GameController.Instance.DisplayQuestion(questionId, OnQuestionAnswered);
        questionDisplayed = true;
    }

    protected override void OnCharacterStay(CharacterControl character)
    {
        if (questionDisplayed || character.IsInvincible )
        {
            return;
        }
        OnCharacterEnter(character);
    }

    protected override void OnCharacterExit(CharacterControl character)
    {
        this.character = null;
        questionDisplayed = false;
    }

    void OnQuestionAnswered(bool result)
    {
        if (result)
        {
            character.UpdateEnergy(GameController.Instance.questionsDB.EnergyAfterRightAnswer);
            StartCoroutine(MoveQuestionToUI(character));
        }
        else
        {
            character.UpdateEnergy(GameController.Instance.questionsDB.EnergyAfterWrongAnswer);
            questionDisplayed = false;
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

        GameController.Instance.UpdateQuestion(1);

        Object.Destroy(this.gameObject);

        yield break;
    }
}
