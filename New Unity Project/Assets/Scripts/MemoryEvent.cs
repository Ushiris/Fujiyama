using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryEvent : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            target.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
