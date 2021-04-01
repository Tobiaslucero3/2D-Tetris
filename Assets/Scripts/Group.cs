using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public static int posy;

    float lastFall = 0;

  //  public Shadow shadow; // The shadow which shows you where it is going to land on the board

    private Store store;

    // Start is called before the first frame update
    void Start()
    {
        // Default position not valid? Then it's game over
        if (IsValidGridPos() != 1)
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }

        store = FindObjectOfType<Store>();
        //shadow = FindObjectOfType<Shadow>();
        //shadow.AssignGroup(this);

    }

    // Update is called once per frame
    void Update()
    {
        posy = (int)(transform.position.y);
        //calculateShadow();

        // Move left
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if the new position is valid
            if(IsValidGridPos() == 1)
            {
                //shadow.UpdateShadowMove(-1);

                UpdateGrid(); // Update the grid since it is valid
            }
            else if(IsValidGridPos() == 2)
            {
              //  shadow.UpdateShadowMove(-1);

                UpdateGridInBetweenBorders();
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        } // Move right
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if the new position is valid
            if (IsValidGridPos() == 1)
            {
               // shadow.UpdateShadowMove(1);

                UpdateGrid(); // Update the grid since it is valid
 
            }
            else if (IsValidGridPos() == 2)
            {
               // shadow.UpdateShadowMove(1);

                UpdateGridInBetweenBorders();
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0); // Its not valid so revert to original position
            }
        } // Rotate
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            // See if the new position is valid
            if (IsValidGridPos() == 1)
            {
                //shadow.UpdateShadowRotate(-90);

                UpdateGrid(); // Update the grid since it is valid
                
            }
            else if (IsValidGridPos() == 2)
            {
               // shadow.UpdateShadowRotate(-90);

                UpdateGridInBetweenBorders();
            }
            else
            {
                transform.Rotate(0, 0, 90); // Its not valid so revert to original position
            }
        } // Fall
        else if(Input.GetKeyDown(KeyCode.DownArrow) || (Time.time - lastFall >=(1.0/Difficulty.GetDifficulty())))
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if the new position is valid
            if (IsValidGridPos() == 1)
            {
                UpdateGrid(); // Update the grid since it is valid
            }
            else
            {
                transform.position += new Vector3(0, 1, 0); // Revert

                // Destroy the shadow
                //shadow.DestroyShadow();

                // Clear filled horizontal lines
                Playfield.deleteFullRows();

                // Spawn next group
                FindObjectOfType<Spawner>().SpawnNext();

                // Disable script
                enabled = false;

                store.ChangeJustStored(false);

            }

            lastFall = Time.time;
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            while(IsValidGridPos() == 1)
            {
                transform.position += new Vector3(0, -1, 0);
            }

            transform.position += new Vector3(0, 1, 0); // Revert by one step since we went too far

            UpdateGrid();

            // Destroy the shadow
            //shadow.DestroyShadow();

            // Clear filled horizontal lines
            Playfield.deleteFullRows();

            // Spawn next group
            FindObjectOfType<Spawner>().SpawnNext();

            // Disable script
            enabled = false;

            store.ChangeJustStored(false);

        }
        else if(Input.GetKeyDown(KeyCode.S))
        {

            if(!store.JustStored())
            {
                // Destroy the shadow
                //  shadow.StoreShadow();

                enabled = false;

                store.StorePiece(this);
            }    
        }
    }



    // Checks if all the blocks within a group have a valid position inside the grid
    // Returns 0 if not valid 1 if valid 2 if in between a border
    int IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            child.position = v;

            // Not inside border?
            if (!Playfield.insideBorder(v))
            {
                if (Playfield.outsideBorder(v))
                {
                    return 2;
                }
                return 0;
            }
            // Block in grid cell (and not part of same group)?
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return 0;
            }
        }
        return 1;
    }

    public int GetYPos()
    {
        int ret = (int)transform.position.y;
        foreach(Transform child in transform)
        {
            if(child.position.y < ret)
            {
                ret = (int)Mathf.Round(child.position.y);
            }
        }
        return ret;
    }

    void UpdateGrid()
    {
        // Remove old children from the grid
        for(int y = 0; y < Playfield.h; ++y)
        {
            for(int x=0; x < Playfield.w; ++x)
            {
                if(Playfield.grid[x,y]!=null)
                {
                    // Checks if a block belongs to this current group 
                    if (Playfield.grid[x, y].parent == transform)
                    {
                        Playfield.grid[x, y] = null;
                    }
                }
            }
        }

        // Add new children to the grid
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // If the group is currently in between borders this is the method that runs
    void UpdateGridInBetweenBorders()
    {
        // Remove old children from the grid
        for (int y = 0; y < Playfield.h; ++y)
        {
            for (int x = 0; x < Playfield.w; ++x)
            {
                if (Playfield.grid[x, y] != null)
                {
                    // Checks if a block belongs to this current group 
                    if (Playfield.grid[x, y].parent == transform)
                    {
                        Playfield.grid[x, y] = null;
                    }
                }
            }
        }
        // Add new children to the grid
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);

            // If outside the left border sets the x component to the width
            if(v.x < 0)
            {
                v.x = Playfield.w - 1;
                child.position = new Vector3(Playfield.w - 1, child.position.y);
            }
            // If outside the rigth border sets the x component to zero
            else if(v.x >= Playfield.w)
            {
                v.x = 0;
                child.position = new Vector3(0, child.position.y);
            }
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }
}


