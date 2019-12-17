using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remind : MonoBehaviour
{
    public GameObject Main;
    MeshFilter mainMesh;
    public List<Mesh> mesh;
    int count = 0;
    public bool stageB = false;
    public List<GameObject> stageBobj;

    
    // Start is called before the first frame update
    void Start()
    {
        mainMesh = Main.GetComponent<MeshFilter>();
    }

    public void remined()
    {
        if (stageB)
        {
            BstageEvent();
        }
        else
        {
            mainMesh.mesh = mesh[count];
        }

        count++;
    }

    private void BstageEvent()
    {
        Vector3 temp = Main.transform.position;
        Main.transform.position = stageBobj[count].transform.position;
        stageBobj[count].transform.position = temp;
        Main = stageBobj[count];
    }
}
