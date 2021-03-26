using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public static int posy;

    float lastFall = 0;

    public Shadow shadow; // The shadow which shows you where it is going to land on the board

    private Store store;

    // Start is called before the first frame update
    void Start()
    {
        // Default position not valid? Then it's game over
        if (!IsValidGridPos(this.gameObject, false))
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }

        store = FindObjectOfType<Store>();
        shadow = FindObjectOfType<Shadow>();
        shadow.AssignGroup(this);

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
            if(IsValidGridPos(this.gameObject, false))
            {
                shadow.updateShadowMove(-1);

                updateGrid(); // Update the grid since it is valid

                
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
            if (IsValidGridPos(this.gameObject, false))
            {
                shadow.updateShadowMove(1);

                updateGrid(); // Update the grid since it is valid

                
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
            if (IsValidGridPos(this.gameObject, false))
            {
                shadow.updateShadowRotate(-90);

                updateGrid(); // Update the grid since it is valid
                
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
            if (IsValidGridPos(this.gameObject, false))
            {
                updateGrid(); // Update the grid since it is valid
            }
            else
            {
                transform.position += new Vector3(0, 1, 0); // Revert

                // Destroy the shadow
                shadow.destroyShadow();

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
            while(IsValidGridPos(this.gameObject, false))
            {
                transform.position += new Vector3(0, -1, 0);
            }

            transform.position += new Vector3(0, 1, 0); // Revert by one step since we went too far

            updateGrid();

            // Destroy the shadow
            shadow.destroyShadow();

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
                shadow.storeShadow();

                enabled = false;

                store.StorePiece(this);
            }    
        }
    }

    void printGrid()
    {
        for (int y = 0; y < Playfield.h; ++y)
        {
            for (int x = 0; x < Playfield.w; ++x)
            {
                if (Playfield.grid[x, y] != null)
                {
                    Debug.Log("1");
                } else
                {
                    Debug.Log("0");
                }
            }
            Debug.Log("\n");
        }
    }

    // Checks if all the blocks within a group have a valid position inside the grid
    bool IsValidGridPos(GameObject obj, bool isShadow)
    {
        foreach (Transform child in obj.transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            //Debug.Log(child.position.y);
            child.position = v;

            // Not inside border?
            if (!Playfield.insideBorder(v))
            {
                return false;
            }

            // Block in grid cell (and not part of same group)?
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != obj.transform)
            {
                return false;
            }

        }
        return true;
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

    void updateGrid()
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

}
