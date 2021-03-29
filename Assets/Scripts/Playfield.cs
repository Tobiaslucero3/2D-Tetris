using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    // Object score;
    private Difficulty difficulty;

    private void Start()
    {
       // score = FindObjectOfType<Score>();
       difficulty = FindObjectOfType<Difficulty>();
    }

    public static int w = 10;
    public static int h = 20;
    public static Transform[,] grid = new Transform[w, h]; // This matrix will keep track
    // of all of the squares in the stage (from (0,0) to (9, 19)) and will store if there is a block on it 

    // This function rounds a vec2 so [1.000001, 2.00002] = [1,2]
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    // This functions check whether the coordinate sent in is within the borders or not
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.y >= 0 && (int)pos.x < w);
    }

    public static void deleteRow(int y)
    {
        for(int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // Decreases a row on a level
    // Decreases row y to row y - 1
    public static void decreaseRow(int y)
    {
        for(int x = 0; x < w; ++x)
        {
            // Check if there is a block in the x and y position
            if(grid[x,y] != null)
            {
                // Move one towards the bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // Decreases all the rows above one that is deleted
    public static void decreaseRowsAbove(int y)
    {
        for(int i = y; i < h; ++i)
        {
            decreaseRow(i);
        }
    }

    // Checks if a row is full
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] == null)
                return false;
        }
        Score.increaseScore(Difficulty.GetDifficulty() *6);
        int score = Score.getScore();
        if((score >= 50) && (score <= 200))
        {
            Difficulty.SetDifficulty(1 + score / 50);
        } else if ((score >= 200) && (score <= 1000))
        {
            Difficulty.SetDifficulty(1 + score / 100);
        }


        return true;
    }

    // This function will delete all the full rows on the board
    public static void deleteFullRows()
    {
        for(int y = 0; y < h; ++y)
        {
            if(isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y+1);
                --y;
            }
        }

    }
}