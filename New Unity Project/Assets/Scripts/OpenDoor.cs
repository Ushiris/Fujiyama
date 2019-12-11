using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    public GameObject sprite;
    public float Duration = 2f;
    public float MoveScale = 10f;
    public string NextScene = "Title";
    public Canvas canvas;
    public bool isWhiteOut;

    public bool IsAnimation = false;
    public List<Sprite> images;
    public List<float> timing;

    public AudioClip openSE;

    Image image;
    PlayerController pl;
    FadeOut fade;
    int count = 0;

    private void Start()
    {
        if(IsAnimation)
        {
            foreach (var item in timing)
            {
                Duration += item;
            }
        }

        fade = gameObject.AddComponent<FadeOut>();
        fade.FadeTime = Duration + 0.2f;
        fade.scene = NextScene;
        fade.isAlone = false;
        fade.isWhite = isWhiteOut;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.white;
            pl = other.GetComponent<PlayerController>();
            pl.isActionable = true;
            pl.EventEffect = Action;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.clear;
            pl.isActionable = false;
        }
    }

    public void Action()
    {
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }
        image = canvas.gameObject.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        fade.panel = image;
        
        pl.SetGoWalkMode(pl.transform.position + Vector3.forward * MoveScale, Duration + 0.5f);
        fade.FadeStart();

        gameObject.AddComponent<AudioSource>().PlayOneShot(openSE);

        if(IsAnimation)
        {
            for(int i=0;i<images.Count;i++)
            {
                Invoke("SwapImage", timing[i]);
            }
        }
    }

    public void SwapImage()
    {
        sprite.GetComponent<SpriteRenderer>().sprite = images[count];
        count++;
    }
}
