using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Quiz quiz;
    [SerializeField] private EndScreen endScreen;

    private void Start()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
    }
    void Update()
    {
        // check if game is complete by looking at quiz class
        // if game over then disable quiz canvas and enable end screen 
        if (quiz.IsGameComplete)
        {
            quiz.gameObject.SetActive(false);
            endScreen.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Debug.Log("restart game button pressed");
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        quiz.RestartGame();
    }
}
