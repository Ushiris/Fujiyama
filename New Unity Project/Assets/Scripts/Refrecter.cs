﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrecter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.Rotate(transform.rotation.eulerAngles);
        collision.gameObject.transform.GetComponent<Rigidbody>().velocity=new;
    }
}
