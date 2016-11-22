using UnityEngine;
using System.Collections;

public class QuestionsPopUpController : MonoBehaviour {

    [SerializeField] UnityEngine.UI.Text title;
    [SerializeField] GameObject answersContainer;
    [SerializeField] GameObject answerPrefab;

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
               
        while (answersContainer.transform.childCount > 0)
        {
            var child = answersContainer.transform.GetChild(0);
            child.gameObject.SetActive(false);
            UnityEngine.UI.Button button = child.GetComponentInChildren<UnityEngine.UI.Button>();
            if (button != null) { button.onClick.RemoveAllListeners(); }
            child.SetParent(null); // unparent object
            Object.Destroy(child.gameObject);
        }

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
        if (onAnswerCb != null)
        {
            onAnswerCb(answerIndex == questionInfo.rightAnswerIndex);
        }
    }
}
