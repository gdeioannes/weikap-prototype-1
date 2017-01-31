using UnityEngine;
using System.Collections;

public class QuestionsPopUpController : MonoBehaviour {

    [SerializeField] UnityEngine.UI.Text title;
    [SerializeField] GameObject answersContainer;
    [SerializeField] GameObject answerPrefab;
    [SerializeField] AnswerPopUpController answerPopUpController;

    private QuestionsDBScriptableObject.Question questionInfo;

    System.Action<bool> onAnswerCb;
    public void ShowQuestion(int questionIndex, System.Action<bool> onAnswerCb)
    {
        this.onAnswerCb = onAnswerCb;
        FillQuestionInfo(questionIndex);
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    void FillQuestionInfo(int questionIndex)
    {
        questionInfo = GameController.Instance.GetQuestionInfoById(questionIndex);
        title.text = questionInfo.Name;

        var buttons = answersContainer.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        answersContainer.transform.DestroyChildren();

        for (int i = 0; i < questionInfo.answers.Length; ++i)
        {
            GameObject answerItem = Object.Instantiate<GameObject>(answerPrefab);
            answerItem.transform.SetParent(answersContainer.transform, false);
            UnityEngine.UI.Text answerText = answerItem.GetComponentInChildren<UnityEngine.UI.Text>();
            answerText.text = questionInfo.answers[i];
            UnityEngine.UI.Button questionButton = answerItem.GetComponentInChildren<UnityEngine.UI.Button>();
            questionButton.onClick.RemoveAllListeners();
            int currentIndex = i; // local variable needed for delegate method
            questionButton.onClick.AddListener(()=> { OnAnswer(currentIndex); });
        }
    }

    public void OnAnswer(int answerIndex)
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
        bool result = answerIndex == questionInfo.rightAnswerIndex;
        if (onAnswerCb != null)
        {
            onAnswerCb(result);
        }
        answerPopUpController.Show(result);
    }
}
