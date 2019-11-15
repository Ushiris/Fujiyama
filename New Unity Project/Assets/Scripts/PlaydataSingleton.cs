using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlaydataSingleton
{
    private static PlaydataSingleton instance;

    public int player_id = 0;
    private string file_dir = Application.dataPath + "/PlayData/" + System.DateTime.Now + ".csv";
    
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

    public void PlayEnd()
    {
        player_id++;
    }

    public void ReachSavePoint()
    {

    }

    public void PlayStart()
    {
        //プレイデータファイルの列を追加。一列目にIDを出力する。

    }

    
}
