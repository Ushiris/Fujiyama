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
    }
}
