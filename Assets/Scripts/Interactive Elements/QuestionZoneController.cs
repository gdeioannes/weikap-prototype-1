using UnityEngine;
using System.Collections;
using DG.Tweening;

public class QuestionZoneController : ConsumableController {
    
    public int coinsAfterRightAnswer;
    public ConsumableController coinControllerPrefab;
    CharacterControl character;
    bool questionDisplayed = false;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        this.character = character;

        if (character.IsInvincible)
        {   // restriction added. unable to interact with questions if invincible
            return;
        }

        GameController.Instance.DisplayQuestion(id, OnQuestionAnswered);
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
            SpawnCoinAndMoveToUI();
			StartCoroutine(IncrementConsumableCountCoroutine(character));            
        }
        else
        {
            character.UpdateEnergy(GameController.Instance.questionsDB.EnergyAfterWrongAnswer);
            questionDisplayed = false;
        }
    }

    void SpawnCoinAndMoveToUI()
    {
        ConsumableController coinController = Object.Instantiate<ConsumableController>(coinControllerPrefab);
        coinController.gameObject.layer = this.gameObject.layer;
        coinController.gameObject.transform.SetParent(this.gameObject.transform.parent, false);
        coinController.gameObject.transform.position = this.transform.position;

        coinController.amount = coinsAfterRightAnswer;
        coinController.type = InGameItemsDBScriptableObject.ItemType.Coin;                
    }		   
}
