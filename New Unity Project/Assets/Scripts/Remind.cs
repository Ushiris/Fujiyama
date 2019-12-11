using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remind : MonoBehaviour
{
    public GameObject Main;
    MeshFilter mainMesh;
    public List<Mesh> mesh;
    int count = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        mainMesh = Main.GetComponent<MeshFilter>();
    }

    public void remined()
    {
        mainMesh.mesh = mesh[count];
        count++;
    }
}
