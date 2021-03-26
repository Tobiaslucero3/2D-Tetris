using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] groups; // A list of all the group game objects with the group script attached

    public GameObject[] groupsNoScripts; // A list of all the group game objects without the group script attached

    public GameObject[] shadows; // A list of all the group game objects with the shadow script attached

    private GameObject next; // This is the game object in the next box without a group script

    private int nextIndex; // This index represents the current index of the piece that is in the next box

    private Vector3 nextPosition; // This is the position where the next box is

    private Vector3[] nextPositions;

    public static Vector3 pos; // This is the position of the bottom of the board where pieces spawn( used to spawn shadows)

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector3(transform.position.x, 0);

        nextPosition = FindObjectOfType<Next>().transform.position;

        nextIndex = Random.Range(0, groups.Length); // Pick a random group

        SpawnFirst(); // Spawns the first group      
    }


    // This function spawns the next object in the scene which will be played with
    public void SpawnNext()
    {
        Instantiate(groups[nextIndex], transform.position, Quaternion.identity);

        Instantiate(shadows[nextIndex], pos, Quaternion.identity);

        Destroy(next);

        nextIndex = Random.Range(0, groups.Length); // Pick a new one

        next = Instantiate(groupsNoScripts[nextIndex], nextPosition, Quaternion.identity);

        next.transform.localScale = new Vector3(0.75f, 0.75f);

    }

    public void SpawnFirst()
    {
        // Spawns the first one

        Instantiate(groups[nextIndex], transform.position, Quaternion.identity);

        Instantiate(shadows[nextIndex], pos, Quaternion.identity);

        nextIndex = Random.Range(0, groups.Length); // Pick a new one

        next = Instantiate(groupsNoScripts[nextIndex], nextPosition, Quaternion.identity);

        next.transform.localScale= new Vector3(0.75f, 0.75f);


    }

}
