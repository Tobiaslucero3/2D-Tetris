using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    private static int difficulty = 1;
    public Text difficultyText;

    // Update is called once per frame
    void Update()
    {
        difficultyText.text = "Difficulty: " + difficulty;
    }

    public static void IncreaseDifficulty()
    {
        difficulty++;
    }

    public static void SetDifficulty(int newDifficulty)
    {
        difficulty = newDifficulty;
    }

    public static int GetDifficulty()
    {
        return difficulty;
    }
}
