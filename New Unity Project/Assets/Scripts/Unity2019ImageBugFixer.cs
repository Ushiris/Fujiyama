using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unity2019ImageBugFixer : MonoBehaviour
{
#if UNITY_2019_1
    private static Unity2019ImageBugFixer instance = null;//シングルトン
    private Sprite sprite;//一時キャッシュ用

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //Update、FixedUpdate、OnRenderObjectだと修正前の画像が一瞬見えてしまう(特にコルーチン)
    private void LateUpdate()
    {
        foreach (Image img in FindObjectsOfType<Image>())
        {
            //シーン上に存在し、spriteが割り当てられているか
            if (img.gameObject.activeInHierarchy && img.sprite != null)
            {
                sprite = img.sprite;
                img.sprite = null;
                img.sprite = sprite;
            }
        }
    }
#else
    private void Awake ()
    {
        Debug.Log ("ImageBugFixerはUnity2019.1のみ動作します");
        Debug.Log ("現在のバージョンでも動作させたい場合はImageBugFixer.csの8行目を「#if UNITY_2019_2」などに変更してください");
        Destroy (gameObject);
    }
#endif
}
