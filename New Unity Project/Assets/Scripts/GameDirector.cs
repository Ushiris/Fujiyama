using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour
{
    public static PlayDataDirector PlayData;

    private void Start()
    {
        PlayData.PlayStart();
    }

    public void OnClickOpenScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public static void OpenScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    //ゲームをシャットダウンします。
    public void OnclickShatDown()
    {
        PlayData.PlayEnd();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
    public static void ShatDown()
    {
        PlayData.PlayEnd();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}
