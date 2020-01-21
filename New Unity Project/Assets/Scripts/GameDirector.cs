using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour
{
    static bool AStageClear = false;
    static bool BStageClear = false;
    public static bool isCleared = true;
    public static int MemoryCount = 0;
    static List<int> MemoryList = new List<int>();

    private void Start()
    {
        //PlayData = gameObject.AddComponent<PlayDataDirector>();
        //PlayData.PlayStart();
    }

    public static bool ClearStage(int stage)
    {
        switch (stage)
        {
            case 1:
                AStageClear = true;
                break;
            case 2:
                BStageClear = true;
                break;
            default:
                break;
        }

        if (AStageClear && BStageClear)
        {
            //PlayData.PlayEnd();
            return true;
        }
        return false;
    }

    public static bool IsEndGame()
    {
        return AStageClear && BStageClear;
    }

    public static bool IsClearGame()
    {
        if (MemoryCount != 6)
        {
            return false;
        }

        return true;
    }

    public static void Remind(int id)
    {
        foreach (var vs in MemoryList)
        {
            if (id == vs)
            {
                return;
            }
        }

        MemoryList.Add(id);
        MemoryCount++;
    }

    static void FlagReset()
    {
        AStageClear = false;
        BStageClear = false;
        MemoryCount = 0;
        MemoryList.Clear();
    }

    public void OnClickOpenScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public static void OpenScene(string SceneName)
    {
        if (SceneName == "Title")
        {
            if (!IsClearGame())
            {
                isCleared = false;
            }

            FlagReset();
        }
        SceneManager.LoadScene(SceneName);
    }

    //ゲームをシャットダウンします。
    public void OnclickShatDown()
    {
        //PlayData.PlayEnd();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
    public static void ShatDown()
    {
        //PlayData.PlayEnd();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}
