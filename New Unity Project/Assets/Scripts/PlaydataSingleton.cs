using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlaydataSingleton
{
    private static PlaydataSingleton instance;
    
    public static PlaydataSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlaydataSingleton();
            }
            return instance;
        }
    }
}
