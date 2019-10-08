/* Made by Ushiris */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject Prev;
    public GameObject Next;
    public CheckPoint PrevCP;
    public CheckPoint NextCP;

    // Start is called before the first frame update
    void Start()
    {
        PrevCP = Prev.GetComponentInChildren<CheckPoint>();
        NextCP = Next.GetComponentInChildren<CheckPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
