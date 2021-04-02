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
        UpdateShadow();
    }

    void UpdateShadow()
    {
        transform.position = Playfield.roundVec2(new Vector2(transform.position.x, 0));

        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);

            if(v.x < 0)
            {
                Debug.Log(child.position.x + " " + v.x);
                child.position = new Vector2(Playfield.w - 1, v.y);
            }
            else if(v.x >= Playfield.w)
            {
                child.position = new Vector2(0, v.y);
            }
            if (v.y < 0)
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

    public void UpdateShadowRotate(int degree)
    {
        transform.Rotate(0, 0, degree);
        UpdateShadow();
    }

    public void UpdateShadowMove(int x)
    {
        transform.position += new Vector3(x, 0);
        UpdateShadow();
    }

    public void DestroyShadow()
    {
        Destroy(gameObject);
        Destroy(this);
    }

    public void StoreShadow()
    {
        if(stored)
        {
            transform.position = Spawner.pos;
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(true);
            stored = false;
            UpdateShadow();
        }
        else
        {
            gameObject.SetActive(false);
            enabled = false;
            stored = true;
        }
    }
}
