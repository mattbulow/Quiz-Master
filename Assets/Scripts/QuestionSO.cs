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

}



//IEnumerator GetDataFromWebpage(string url)
//{
//    WWW webpage = new WWW(url);
//    while (!webpage.isDone) yield return false;
//    string content = webpage.text;
//    . . .
//    // Do something with <content>
//}