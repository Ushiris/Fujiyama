using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
    //タイムオーバー時の処理に使う型
    public delegate void TimeOver();
    
    public float limit = 25; //デフォルトを25で固定（わけわからんイベントで混乱するのを防止）
    public TimeOver timeOut;
    public bool isLoop = false;

    float timer = 0f;
    bool isActive = false;
    
    void Start()
    {
        timeOut = noAction; //初期化（null refにならんようにする）
    }

    //カウントアップ形式での時間計測とイベントの発火
    void Update()
    {
        if(isActive)
        {
            timer += Time.deltaTime;

            if (timer >= limit)
            {
                //デフォルト時間の場合は警告
                if(limit==25)
                {
                    Debug.Log(gameObject.name + ":このタイマーデフォルトの時間だけど大丈夫？");
                }
                timeOut();
                timer -= limit;//リセット
            }
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    public void pause()
    {
        isActive = false;
    }

    public void resume()
    {
        isActive = true;
    }

    public void noAction()
    {
        //何もしない
    }
}
