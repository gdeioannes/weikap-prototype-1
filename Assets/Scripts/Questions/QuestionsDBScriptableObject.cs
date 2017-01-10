using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewQuestionsDB", menuName = "Weikap/DB/Questions")]
public class QuestionsDBScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct Question
    {
        public string Name;
        public int rightAnswerIndex;
        public string[] answers;
    }

    public int EnergyAfterRightAnswer;
    public int EnergyAfterWrongAnswer;

    public Question[] questions;

    public bool IsRightAnswer(int questionId, int answerIndex)
    {
        if (questions.Length <= questionId)
        {
            return false; // invalid question id
        }

        return questions[questionId].rightAnswerIndex == answerIndex;
    }

    public Question GetQuestionInfoById(int questionId)
    {
        if (questions.Length <= questionId)
        {
            return default(Question); // invaild question id
        }

        return questions[questionId];
    }
}
