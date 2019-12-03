using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource source;

    public AudioClip enter;
    public AudioClip over;


    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEnter()
    {
        source.PlayOneShot(enter);
    }

    public void PlayOver()
    {
        source.PlayOneShot(over);
    }
}
