using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private static int score = 0;
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public static void increaseScore(int increase)
    {
        score += increase;
    }

    public static int getScore()
    {
        return score;
    }
}
