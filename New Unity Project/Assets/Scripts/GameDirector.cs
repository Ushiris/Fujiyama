using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour
{
    public static PlayDataDirector PlayData;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void OpenScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    //ゲームをシャットダウンします。
    public void ShatDown()
    {
        PlayData.PlayEnd();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}
