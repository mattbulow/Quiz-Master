using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    TriviaQuestionsJson questionDataJson = new TriviaQuestionsJson();
    int currentQuestionIdx = 0;

    [Header("Questions")]
    private QuestionSO currentQuestion;
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
    private bool isAnsweringQuestion = false;
    private bool gettingQuestionDataOngoing = true;

    void Start()
    {
        currentQuestion = new QuestionSO();
        Debug.Log("currentQuestion == null: " + currentQuestion == null);
        StartCoroutine(GetQuestionsFromWeb());
        //while (coroutine != null) { } // wait until coroutine finishes
    }

    void Update()
    {
        if (gettingQuestionDataOngoing)
        {
            return;
        }

        // If we are answering a question and time is up, then display correct answer and reset timer
        if (isAnsweringQuestion && timer.GetTimeUp())
        {
            OnAnswerSelected(-1);
        }
        // If we are reviewing a question and time is up, then get next question (which will reset timer)
        else if (!isAnsweringQuestion && timer.GetTimeUp())
        {
            if (currentQuestionIdx < questionDataJson.results.Length)
            {
                GetNextQuestion();
            } else
            {
                Debug.Log("End of Game");
            }
        }
    }

    private void OnAnswerSelected(int idxOfSelectedButton)
    {
        Image correctAnswerButtonImage = answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>();
        if (correctAnswerButtonImage == null) { Debug.LogError("correctAnswerButtonImage is NULL"); }
        string correctAnswerString = correctAnswerButtonImage.GetComponentInChildren<TextMeshProUGUI>().text;
        if (correctAnswerString == null) { Debug.LogError("correctAnswerString is NULL"); }

        if (idxOfSelectedButton == currentQuestion.GetCorrectAnswerIndex())
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
        currentQuestion.UpdateQuestion(questionDataJson.results[currentQuestionIdx]);
        //currentQuestion.UpdateQuestion
        DisplayQuestion();
        isAnsweringQuestion = true;
        timer.ResetTimer(isAnsweringQuestion);
        currentQuestionIdx++;
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int n = 0; n < answerButtons.Length; n++)
        {
            TextMeshProUGUI buttonText = answerButtons[n].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText == null) { Debug.LogError("buttonText is NULL"); }
            buttonText.text = currentQuestion.GetAnswer(n);
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


    IEnumerator GetQuestionsFromWeb()
    {
        gettingQuestionDataOngoing = true;
        //int numberOfQuestions = 2;
        UnityWebRequest www = UnityWebRequest.Get("https://opentdb.com/api.php?amount=10&difficulty=easy&type=multiple&category=15");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            questionDataJson = JsonUtility.FromJson<TriviaQuestionsJson>(www.downloadHandler.text);
        }
        gettingQuestionDataOngoing = false;
    }
}
