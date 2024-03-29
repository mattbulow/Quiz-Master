using TMPro;
using UnityEngine;


public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private ScoreKeeper scoreKeeper;

    // need to display score in text, get from score manager

    private void Update()
    {
        text.text = string.Format("Congrats!\nYou scored: {0:f0}%", scoreKeeper.Score);
    }
}
