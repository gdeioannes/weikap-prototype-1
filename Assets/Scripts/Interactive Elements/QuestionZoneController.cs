using UnityEngine;
using System.Collections;

public class QuestionZoneController : BaseInteractiveElement {

    [SerializeField] int questionId;

    protected override void OnCharacterEnter(CharacterControl character)
    {
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        // TODO: NEED TO BE DEFINED
        Object.Destroy(this.gameObject);
    }
}
