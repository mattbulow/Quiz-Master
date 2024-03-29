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

    [Header("Score Keeper")]
    [SerializeField] private ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] private Slider slider;

    // non serialized fields
    private bool isAnsweringQuestion = false;
    private bool gettingQuestionDataOngoing = true;
    public bool IsGameComplete { get; private set; } = false;

    public void RestartGame()
    {
        gettingQuestionDataOngoing = true;
        currentQuestionIdx = 0;
        IsGameComplete = false;
        scoreKeeper.ResetScore();
        StartCoroutine(GetQuestionsFromWeb());
    }

    void Start()
    {
        currentQuestion = ScriptableObject.CreateInstance<QuestionSO>();
        Debug.Log("currentQuestion == null: " + currentQuestion == null);
        gettingQuestionDataOngoing = true;
        StartCoroutine(GetQuestionsFromWeb());
    }

    void Update()
    {
        if (gettingQuestionDataOngoing)
        {
            //TODO: display some text like: downloading questions from opentdb.com...
            // might want to do this in the coroutine...
            return;
        }

        // If we are answering a question and time is up, then display correct answer and reset timer
        if (isAnsweringQuestion && timer.GetTimeUp())
        {
            OnAnswerSelected(-1);
        }
        // If we are reviewing a question and time is up, then get next question (which will reset timer)
        else if (!isAnsweringQuestion && (timer.GetTimeUp() || Input.GetMouseButtonDown(0)) )
        {
            // when clicking mouse, skip to next question
            if (currentQuestionIdx < questionDataJson.results.Length)
            {
                GetNextQuestion();
            } else
            {
                IsGameComplete = true;
            }
        }
    }

    public void OnAnswerSelected(int idxOfSelectedButton)
    {
        Image correctAnswerButtonImage = answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>();
        if (correctAnswerButtonImage == null) { Debug.LogError("correctAnswerButtonImage is NULL"); }
        string correctAnswerString = correctAnswerButtonImage.GetComponentInChildren<TextMeshProUGUI>().text;
        if (correctAnswerString == null) { Debug.LogError("correctAnswerString is NULL"); }

        if (idxOfSelectedButton == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Good job!, \"" + correctAnswerString + "\" is CORRECT!";
            scoreKeeper.IncrementAnswers(true);
        }
        else if (idxOfSelectedButton == -1)
        {
            questionText.text = "Sorry, time is up! The correct answer was: \"" + correctAnswerString + "\"";
            scoreKeeper.IncrementAnswers(false);
        } else
        {
            questionText.text = "Sorry, \"" +
                answerButtons[idxOfSelectedButton].GetComponentInChildren<TextMeshProUGUI>().text +
                "\" is INCORRECT, the correct answer was: \"" +
                correctAnswerString + "\"";
            scoreKeeper.IncrementAnswers(false);
        }

        correctAnswerButtonImage.sprite = correctAnswerSprite;

        SetAnswerButtonsState(false);
        isAnsweringQuestion = false;
        timer.ResetTimer(isAnsweringQuestion);
    }

    private void SetAnswerButtonsStateTrue() { SetAnswerButtonsState(true); }

    private void GetNextQuestion()
    {
        Invoke("SetAnswerButtonsStateTrue",0.25f); // this is to prevent OnClick() of a button occurring when skipping the review part of a question
        SetDefaultButtonSprites();
        currentQuestion.UpdateQuestion(questionDataJson.results[currentQuestionIdx]);
        //currentQuestion.UpdateQuestion
        DisplayQuestion();
        isAnsweringQuestion = true;
        timer.ResetTimer(isAnsweringQuestion);
        currentQuestionIdx++;
        slider.value = currentQuestionIdx;
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

    private void ClearAnswerButtonText()
    {
        for (int n = 0; n < answerButtons.Length; n++)
        {
            TextMeshProUGUI text = answerButtons[n].GetComponentInChildren<TextMeshProUGUI>();
            text.text = "";
        }
    }


    IEnumerator GetQuestionsFromWeb()
    {
        // display getting data from internet and clear button text
        questionText.text = "Retrieving trivia question from opentdb.com";
        ClearAnswerButtonText();

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

        // this needs to be here or else questionDataJson will be empty
        slider.minValue = 0;
        slider.maxValue = questionDataJson.results.Length;
        slider.value = 1;

        gettingQuestionDataOngoing = false;
    }

}
