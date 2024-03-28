using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question",fileName = "New Question")]

public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] private string question = "Enter new question text here";
    [SerializeField] private string[] answers = { "answer 1", "answer 2", "answer 3", "answer 4" };
    [SerializeField] int correctAnswerIndex;

    public string GetQuestion() { return question; }
    public string GetAnswer(int index) { return answers[index]; }
    public int GetCorrectAnswerIndex() { return correctAnswerIndex; }

    public void UpdateQuestion(QuestionJson questionJson)
    {
        // take the QuestionJson and convert it to QuestionSO format. alternatively, could have built QuestionSO to match opentdb.com to begin with
        question = questionJson.question;

        correctAnswerIndex = Random.Range(0, answers.Length);
        answers[correctAnswerIndex] = questionJson.correct_answer;
        int m = 0;
        for (int n = 0; n< answers.Length; n++)
        {
            if (n == correctAnswerIndex) { continue; }
            answers[n] = questionJson.incorrect_answers[m++];
        }
    }

}



//IEnumerator GetDataFromWebpage(string url)
//{
//    WWW webpage = new WWW(url);
//    while (!webpage.isDone) yield return false;
//    string content = webpage.text;
//    . . .
//    // Do something with <content>
//}