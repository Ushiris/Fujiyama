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
            isActive = true;
            player.SetGoWalkMode(DeskPos, AnimDuration);
            fade = gameObject.AddComponent<SimpleFade>();
            fade.Setting(AnimDuration, panel, GameDirector.IsClearGame());
            fade.set(true);

            if (GameDirector.IsClearGame())
            {
                Invoke("Turn", 2.0f);
            }
            else
            {
                dark_room.color = Color.white;
                Invoke("EndGame", 2.0f);
            }
        }
    }

    void Turn()
    {
        player.SetGoWalkMode(DoorPos, AnimDuration);
        door_open.color = Color.white;
        Invoke("EndGame",0.5f);
    }

    void EndGame()
    {
        GameDirector.OpenScene("Title");
    }
}
