using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEmargencyMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.U))
        {
            GameDirector.OpenScene("Title");
        }

        if(Input.GetKey(KeyCode.C)&&Input.GetKey(KeyCode.S)&&Input.GetKey(KeyCode.A))
        {
            GameDirector.ClearStage(1);
        }

        if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.B))
        {
            GameDirector.ClearStage(2);
        }

        if(Input.GetKey(KeyCode.M)&&Input.GetKey(KeyCode.A))
        {
            GameDirector.MemoryCount = 6;
        }

        if(Input.GetKey(KeyCode.E)&& Input.GetKey(KeyCode.N)&& Input.GetKey(KeyCode.D))
        {
            GameDirector.OpenScene("Select");
        }
    }
}
