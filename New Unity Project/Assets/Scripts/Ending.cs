using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public PlayerController player;
    public Vector3 DeskPos;
    public Vector3 DoorPos;

    public float AnimDuration;
    public Image panel;
    public SpriteRenderer door_open;
    public SpriteRenderer dark_room;

    bool isActive = false;
    SimpleFade fade;

    // Start is called before the first frame update
    void Start()
    {
        if (GameDirector.IsEndGame())
        {
            player.Bind();
            player.Look(DeskPos);
            Invoke("Setup", 0.5f);

            if (GameDirector.IsClearGame())
            {
                Invoke("OpenDoor",1.5f);
                Invoke("Turn", 2.0f);
                Invoke("FadeStart", 3.0f);
                Invoke("EndGame", 5.0f);
            }
            else
            {
                dark_room.color = Color.white;
                Invoke("EndGame", 2.0f);
            }
        }
    }

    void Setup()
    {
        isActive = true;
        player.SetGoWalkMode(DeskPos, AnimDuration);
        fade = gameObject.AddComponent<SimpleFade>();
        fade.Setting(2.0f, panel, GameDirector.IsClearGame());
        fade.set(false);
    }

    void Stop()
    {
        player.CancelMovie();
        player.Bind();
    }

    void OpenDoor()
    {
        door_open.color = Color.white;
    }

    void FadeStart()
    {
        fade.set(true);
    }

    void Turn()
    {
        player.Look(DoorPos);
        Stop();
        door_open.color = Color.white;
    }

    void EndGame()
    {
        GameDirector.OpenScene("Title");
    }
}
