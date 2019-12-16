using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public GameObject prev;
    public GameObject to;
    public CheckPoint p_cp;
    public CheckPoint t_cp;
    public LR look = LR.right;

    // Start is called before the first frame update
    void Start()
    {
        p_cp = prev.GetComponentInChildren<CheckPoint>();
        t_cp = to.GetComponentInChildren<CheckPoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.gameObject.GetComponent<PlayerController>().SavePoint = this;
        }
    }
}
