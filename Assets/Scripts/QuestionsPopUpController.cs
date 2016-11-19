using UnityEngine;
using System.Collections;

public class QuestionsPopUpController : MonoBehaviour {

    System.Action<bool> onAnswerCb;
    public void ShowQuestion(int questionIndex, System.Action<bool> onAnswerCb)
    {
        this.onAnswerCb = onAnswerCb;
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnAnswer(int answerIndex)
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;

        if (this.onAnswerCb != null)
        {
            this.onAnswerCb(true);
        }
    }
}
