﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remind : MonoBehaviour
{
    public GameObject Main;
    public List<Mesh> mesh;
    public bool stageB = false;
    public List<GameObject> stageBobj;
    public RotateObj rotateObj;
    
    int count = 0;
    MeshFilter mainMesh;

    
    // Start is called before the first frame update
    void Start()
    {
        mainMesh = Main.GetComponent<MeshFilter>();
    }

    public void Remined()
    {
        if (stageB)
        {
            BstageEvent();
        }
        else
        {
            mainMesh.mesh = mesh[count];
            rotateObj.isActive = true;
        }

        count++;
    }

    //Bステージ版への対応
    private void BstageEvent()
    {
        Vector3 temp = Main.transform.position;
        Main.transform.position = stageBobj[count].transform.position;
        stageBobj[count].transform.position = temp;
        Main = stageBobj[count];
    }
}
