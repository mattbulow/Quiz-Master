using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timerValue = 30f;
    [SerializeField] private float timeToAnswerQuestion = 30f;
    [SerializeField] private float timeToReviewCorrectAnswer = 10f;
    float timerFullTime;

    [SerializeField] private Image timerImage;
    private bool pause = false;
    private bool timeUp = false;

    private void Start()
    {
        timerValue = timerFullTime = timeToAnswerQuestion;
    }
    void Update()
    {
        if (!pause && !timeUp)
        {
            UpdateTimer();
        }
        
    }

    public void ResetTimer(bool isAnsweringQuestion)
    {
        if (isAnsweringQuestion)
        {
            timerValue = timerFullTime = timeToAnswerQuestion;
        }
        else
        {
            timerValue = timerFullTime = timeToReviewCorrectAnswer;
        }
        timeUp = false;
    }

    public void PauseTimer (bool input) { pause = input; }
    public bool GetTimeUp() { return timeUp; }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;
        if (timerValue <= 0)
        {
            timerValue = 0;
            timeUp = true;
        }
        timerImage.fillAmount = timerValue / timerFullTime;
    }

}
