using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    private Text scoreText;

    protected override void OnAwake()
    {
        scoreText = transform.Find("ScoreLabel/ScoreText").GetComponent<Text>();
    }

    public void UpdateScore(float score)
    {
        scoreText.text = ((int) score).ToString();
    }
}