using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private QuestionSO question;
    [SerializeField] private TextMeshProUGUI questionText;

    [Header("Answers")]
    [SerializeField] private GameObject[] answerButtons;

    [Header("Button Colors")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;

    [Header("Timers")]
    [SerializeField] Image timerImage;
    [SerializeField] Timer timer;


    // non serialized fields
    private bool isAnsweringQuestion = true;

    void Start()
    {
        GetNextQuestion();
    }

    void Update()
    {
        // If we are answering a question and time is up, then display correct answer and reset timer
        if (isAnsweringQuestion && timer.GetTimeUp())
        {
            OnAnswerSelected(-1);
        }
        // If we are reviewing a question and time is up, then get next question (which will reset timer)
        else if (!isAnsweringQuestion && timer.GetTimeUp())
        {
            GetNextQuestion();
        }
    }

    private void OnAnswerSelected(int idxOfSelectedButton)
    {
        Image correctAnswerButtonImage = answerButtons[question.GetCorrectAnswerIndex()].GetComponent<Image>();
        if (correctAnswerButtonImage == null) { Debug.LogError("correctAnswerButtonImage is NULL"); }
        string correctAnswerString = correctAnswerButtonImage.GetComponentInChildren<TextMeshProUGUI>().text;
        if (correctAnswerString == null) { Debug.LogError("correctAnswerString is NULL"); }

        if (idxOfSelectedButton == question.GetCorrectAnswerIndex())
        {
            questionText.text = "Good job!, \"" + correctAnswerString + "\" is CORRECT!";
        }
        else if (idxOfSelectedButton == -1)
        {
            questionText.text = "Sorry, time is up! The correct answer was: \"" + correctAnswerString + "\"";
        } else
        {
            questionText.text = "Sorry, \"" +
                answerButtons[idxOfSelectedButton].GetComponentInChildren<TextMeshProUGUI>().text +
                "\" is INCORRECT, the correct answer was: \"" +
                correctAnswerString + "\"";
        }

        correctAnswerButtonImage.sprite = correctAnswerSprite;
        SetAnswerButtonsState(false);
        isAnsweringQuestion = false;
        timer.ResetTimer(isAnsweringQuestion);
    }

    private void GetNextQuestion()
    {
        SetAnswerButtonsState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
        isAnsweringQuestion = true;
        timer.ResetTimer(isAnsweringQuestion);
    }
    private void DisplayQuestion()
    {
        questionText.text = question.GetQuestion();

        for (int n = 0; n < answerButtons.Length; n++)
        {
            TextMeshProUGUI buttonText = answerButtons[n].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText == null) { Debug.LogError("buttonText is NULL"); }
            buttonText.text = question.GetAnswer(n);
        }
    }
    private void SetAnswerButtonsState(bool state)
    {
        for (int n = 0;n < answerButtons.Length;n++)
        {
            Button button = answerButtons[n].GetComponent<Button>();
            button.interactable = state;
        }
    }
    private void SetDefaultButtonSprites()
    {
        for (int n = 0; n < answerButtons.Length; n++)
        {
            Image image = answerButtons[n].GetComponent<Image>();
            image.sprite = defaultAnswerSprite;
        }
    }

}
