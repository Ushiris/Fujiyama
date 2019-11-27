using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    title,
    select,
    stageA,
    stageB,
    result
};


public class GameDirector : MonoBehaviour
{
    public static string
        title,
        select,
        stageA,
        stageB,
        result;

    public static PlayDataDirector PlayData;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void OpenScene(SceneName scene)
    {
        string name;
        switch (scene)
        {
            case SceneName.title:
                name = title;
                break;
            case SceneName.select:
                name = select;
                break;
            case SceneName.stageA:
                name = stageA;
                break;
            case SceneName.stageB:
                name = stageB;
                break;
            case SceneName.result:
                name = result;
                break;
            default:
                name = title;
                break;
        }

        SceneManager.LoadScene(name);
    }

    public void ShatDown()
    {

    }
}
