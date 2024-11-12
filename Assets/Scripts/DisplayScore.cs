using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();

        scoreText.text = $"Final Score: {StaticData.FinalPoints} Points";
    }
}
