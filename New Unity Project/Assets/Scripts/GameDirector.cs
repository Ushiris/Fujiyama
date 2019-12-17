using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour
{
    static PlayDataDirector PlayData;
    static bool AStageClear = false;
    static bool BStageClear = false;
    static List<bool> MemoryFragmentFlag=new List<bool>(6);

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

        if(AStageClear&&BStageClear)
        {
            //PlayData.PlayEnd();
            return true;
        }
        return false;
    }

    public static bool IsClearGame()
    {
        foreach(var fg in MemoryFragmentFlag)
        {
            if(!fg)
            {
                return false;
            }
        }

        return true;
    }

    public static void Remind(int id)
    {
        MemoryFragmentFlag[id] = true;
    }

    static void FlagReset()
    {
        AStageClear = false;
        BStageClear = false;
        MemoryFragmentFlag.Clear();
        MemoryFragmentFlag = new List<bool>(6);
    }

    public void OnClickOpenScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public static void OpenScene(string SceneName)
    {
        if(SceneName=="Title")
        {
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
