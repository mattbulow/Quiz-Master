[System.Serializable]
public class QuestionJson
{
    public string type;
    public string difficulty;
    public string category;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}

[System.Serializable]
public class TriviaQuestionsJson
{
    public int response_code = -1;
    public QuestionJson[] results;
}