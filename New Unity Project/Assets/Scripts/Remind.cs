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
    public List<Material> stageBmat;
    
    // Start is called before the first frame update
    void Start()
    {
        mainMesh = Main.GetComponent<MeshFilter>();
    }

    public void remined()
    {
        mainMesh.mesh = mesh[count];
        count++;
        if(stageB)
        {
            BstageEvent();
        }
    }

    private void BstageEvent()
    {
        Main.GetComponent<MeshRenderer>().materials[0] = stageBmat[count*2];
        Main.GetComponent<MeshRenderer>().materials[1] = stageBmat[count*2+1];
    }

}
