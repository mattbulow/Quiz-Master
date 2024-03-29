using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int correctAnswers = 0;
    private int questionsAnswered = 0;
    public float Score { get; private set; } = 0;
   
    void Start()
    {
        scoreText.text = "Score:";
    }



    public void IncrementAnswers(bool isAnswerCorrect)
    {
        questionsAnswered++;
        if (isAnswerCorrect) { correctAnswers++; }
        Score = correctAnswers / (float)questionsAnswered * 100;
        scoreText.text = string.Format("Score: {0:f0}%", Score);
    }

    public void ResetScore()
    {
        correctAnswers = 0;
        questionsAnswered = 0;
        scoreText.text = "Score:";
    }


}
