using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    private Group store = null;

    private bool isFull = false;

    private bool justStored = false;

    private Spawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    // This is used to store a piece from the board to the store box
    public void StorePiece(Group obj)
    {
        justStored = true;
        // This means that there is a piece already being stored
        if(!isFull)
        {
            spawner.SpawnNext(); // Spawns the next piece
            isFull = true; // From now on, it is always full
        }
        else
        {
            store.transform.localScale = new Vector3(1,1,1); // Spawn the stored piece

            store.transform.position = spawner.transform.position; // Sets the stored piece position to the spawned piece

            store.enabled = true; // Enables the Group script 

            store.shadow.enabled = true;
            store.shadow.storeShadow();
        }
        
        store = obj; // Saves the object being stored

        // 
        for (int y = 0; y < Playfield.h; ++y)
        {
            for (int x = 0; x < Playfield.w; ++x)
            {
                if (Playfield.grid[x, y] != null)
                {
                    // Checks if a block belongs to this current group 
                    if (Playfield.grid[x, y].parent == store.transform)
                    {
                        Playfield.grid[x, y] = null;
                    }
                }
            }
        }

        
        store.transform.localScale = new Vector3(0.75f, 0.75f, 1);
        store.transform.rotation = Quaternion.identity;
        store.transform.position = this.transform.position;

        //Instantiate(store.gameObject, transform.position, Quaternion.identity);

    }

    public bool JustStored()
    {
        return justStored;
    }

    public void ChangeJustStored(bool boolean)
    {
        justStored = boolean;
    }
}
