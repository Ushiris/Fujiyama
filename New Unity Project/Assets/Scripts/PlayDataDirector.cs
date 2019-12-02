using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerEvents
{
    PlayStart,
    StageEnter,
    Lost,
    SectionClear,
    StageClear,
    BreakTimeEnter,
    BreakTimeExit,
    GameExit,
};


public class PlayDataDirector : MonoBehaviour
{
    public int player_id = 0;
    private string file_dir = Application.dataPath + "/PlayData/" + System.DateTime.Now + ".csv";
    const int STAGE_A_SECTION_COUNT = 12;
    const int STAGE_B_SECTION_COUNT = 12;

    bool isClearStageA = false;
    bool isClearStageB = false;
    float AppRunningTime = 0f;
    float StagePlayTime = 0f;
    float SavePointTimer = 0f;
    float BreakTime = 0f;
    int LostCount = 0;

    List<float?> SectionClearTimes = new List<float?>(STAGE_A_SECTION_COUNT);
    List<float?> SectionBreakTimes = new List<float?>(STAGE_B_SECTION_COUNT);
    List<int?> SectionLostCounts = new List<int?>(STAGE_A_SECTION_COUNT + STAGE_B_SECTION_COUNT);

    private void Start()
    {
        SectionClearTimes.ForEach(n => n = null);
        SectionBreakTimes.ForEach(n => n = null);
        SectionLostCounts.ForEach(n => n = null);
    }

    // Update is called once per frame
    void Update()
    {
        AppRunningTime += Time.deltaTime;
    }

    public void PlayEnd()
    {
        //セクションの途中でやめた場合の処理


        //リセット処理
        player_id++;
        isClearStageA = false;
        isClearStageB = false;
        AppRunningTime = 0f;
        StagePlayTime = 0f;
        SavePointTimer = 0f;
        BreakTime = 0f;
        LostCount = 0;

        //ログデータの書き出しを行う。
    }

    public void PlayStart()
    {
        //プレイデータファイルの列を追加。一列目にIDを出力する。
        AppRunningTime = 0f;
    }
    
    public void ReachSavePoint()
    {
        SavePointTimer = 0f;
    }
}
