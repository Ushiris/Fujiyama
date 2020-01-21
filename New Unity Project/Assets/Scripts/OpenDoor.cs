using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    //基本的なパラメータ
    public GameObject sprite;
    public float Duration = 2f;
    public float MoveScale = 10f;
    public string NextScene = "Title";
    public Canvas canvas;
    public bool isWhiteOut;
    public AudioClip openSE;

    //アニメーションする必要がある場合への対応
    public bool IsAnimation = false;
    public List<Sprite> images;
    public List<float> timing;


    Image image;
    PlayerController pl;
    FadeOut fade;
    int count = 0;

    private void Start()
    {
        //（アニメーション有りなら）ドア開放アニメーションのセットアップ
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
        //プレイヤーにアクションを与える
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
        //プレイヤーがアクションを起こせなくする
        if (other.tag == "Player")
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.clear;
            pl.isActionable = false;
        }
    }

    //プレイヤーが行うアクション。ムービーモードへ移行し、フェードアウトする。
    public void Action()
    {
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }
        if (image == null)
        {
            image = canvas.gameObject.AddComponent<Image>();
        }
        image.color = new Color(0, 0, 0, 0);
        fade.panel = image;

        pl.isActionable = false;
        pl.Look(new Vector3(pl.transform.position.x, pl.transform.position.y, pl.transform.position.z + 1));
        pl.Bind();
        fade.FadeStart();

        gameObject.AddComponent<AudioSource>().PlayOneShot(openSE);

        //アニメーションが存在する場合の処理
        if(IsAnimation)
        {
            for(int i=0;i<images.Count;i++)
            {
                Invoke("SwapImage", timing[i]);
            }
        }
        Destroy(this);
    }

    public void SwapImage()
    {
        sprite.GetComponent<SpriteRenderer>().sprite = images[count];
        count++;
    }
}
