using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] private QuestionSO question;
    [SerializeField] private TextMeshProUGUI questionText;

    [SerializeField] private GameObject[] answerButtons;

    int correctAnswerIdx;

    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;


    void Start()
    {
        GetNextQuestion();
        //DisplayQuestion();
    }

    public void OnAnswerSelected(int idxOfSelectedButton)
    {
        Image correctAnswerButtonImage = answerButtons[question.GetCorrectAnswerIndex()].GetComponent<Image>();
        if (correctAnswerButtonImage == null) { Debug.LogError("correctAnswerButtonImage is NULL"); }
        string correctAnswerString = correctAnswerButtonImage.GetComponentInChildren<TextMeshProUGUI>().text;
        if (correctAnswerString == null) { Debug.LogError("correctAnswerString is NULL"); }

        if (idxOfSelectedButton == question.GetCorrectAnswerIndex())
        {
            questionText.text = "Good job!, \"" + correctAnswerString + "\" is CORRECT!";
        }
        else
        {
            questionText.text = "Sorry, \"" +
                answerButtons[idxOfSelectedButton].GetComponentInChildren<TextMeshProUGUI>().text +
                "\" is INCORRECT, the correct answer is: \"" +
                correctAnswerString + "\"";
        }

        correctAnswerButtonImage.sprite = correctAnswerSprite;
        SetAnswerButtonsState(false);
    }

    private void GetNextQuestion()
    {
        SetAnswerButtonsState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
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
