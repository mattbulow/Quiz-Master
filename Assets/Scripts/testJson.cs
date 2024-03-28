using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class testJson : MonoBehaviour
{
    TriviaQuestions questionData = new TriviaQuestions();
    [SerializeField] int counter;

    void Start()
    {
        StartCoroutine(GetText());

        // To convert the JSON back into an object, use JsonUtility.FromJson:
        //myObject = JsonUtility.FromJson<MyClass>(json);
    }

    private void Update()
    {
          if (questionData.response_code == 0)
        {
            //Debug.Log("Response Code: " + questionData.response_code);
            //foreach (Question question in questionData.results)
            //{
            //    Debug.Log("Question: " + question.question);
            //    Debug.Log("Correct Answer: " + question.correct_answer);
            //    Debug.Log("Incorrect Answers: ");
            //    foreach (string incorrectAnswer in question.incorrect_answers)
            //    {
            //        Debug.Log("- " + incorrectAnswer);
            //    }
            //}
        }else
        {
            counter++;
            //Debug.LogWarning("Response Code: " + questionData.response_code);
        }
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://opentdb.com/api.php?amount=10&difficulty=easy&type=multiple&category=15");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            string json_str = www.downloadHandler.text;
            questionData = JsonUtility.FromJson<TriviaQuestions>(json_str);
        }
    }
}

[System.Serializable]
public class Question
{   
    public string type;
    public string difficulty;
    public string category;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}

[System.Serializable]
public class TriviaQuestions
{
    public int response_code = -1;
    public Question[] results;
}