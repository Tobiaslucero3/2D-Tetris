using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    Group parent;

    bool stored = false;

    private void Start()
    {
        updateShadow();
    }

    void updateShadow()
    {
        transform.position = Playfield.roundVec2(new Vector2(transform.position.x, 0));

        foreach (Transform child in transform)
        {
            if (child.position.y < 0)
            {
                transform.position += new Vector3(0, 1);
            }
        }

        int childCount = transform.childCount;
        int j = 0;
        int pos;
        foreach (Transform child in transform)
        {

            pos = (int)Mathf.Round(child.position.y);
            if(pos==0)
            {
                //Debug.Log("Broke");
                break;
            }
            if(j == childCount - 1)
            {
                transform.position -= new Vector3(0, 1);
                break;
            }
            ++j;

        }

        bool done = false;
        j = 0;
        int posx = 0;
        
        while ((!done) && (transform.position.y < Playfield.h))
        {
           // Debug.Log("Ran");
            foreach (Transform child in transform)
            {
                pos = (int)Mathf.Round(child.position.y);
                posx = (int)Mathf.Round(child.position.x);
                if (Playfield.grid[posx,pos] != null)
                {
                    transform.position += new Vector3(0, 1);
                    //Debug.Log("x: " + child.position.x + "y: " +child.position.y);
   
                    break;
                }
                else
                {
                    if (j == childCount - 1)
                    {
                        done = true;
                    }
                }
                ++j;
            }
            j = 0;
        }

        int move = 0;
        foreach(Transform child in transform)
        {
            posx = (int)Mathf.Round(child.position.x);
            pos = (int)Mathf.Round(child.position.y);
            for (int i = pos + 1; i < parent.GetYPos() - 1; ++i)
            {
                //Debug.Log("ran");
                if (Playfield.grid[posx, i] != null)
                {   
                    if((i + 1) - pos > move)
                    {
                        move = (i + 1) - pos;
                    }
                }
            }
        }
        transform.position += new Vector3(0, move);
    }

    public void AssignGroup(Group group)
    {
        parent = group;
    }

    public void updateShadowRotate(int degree)
    {
        transform.Rotate(0, 0, degree);
        //transform.position = Playfield.roundVec2(transform.position);
        updateShadow();
    }

    public void updateShadowMove(int x)
    {
        transform.position += new Vector3(x, 0);
        updateShadow();
    }

    public void destroyShadow()
    {
        Destroy(gameObject);
        Destroy(this);
    }

    public void storeShadow()
    {
        if(stored)
        {
            transform.position = Spawner.pos;
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(true);
            stored = false;
            updateShadow();
        }
        else
        {
            gameObject.SetActive(false);
            enabled = false;
            stored = true;
        }
    }
}
