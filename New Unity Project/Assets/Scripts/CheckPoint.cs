using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject Prev;
    public GameObject Next;
    public CheckPoint PrevCP;
    public CheckPoint NextCP;
    public Vector3 ToNext;
    public Vector3 ToPrev;
    public float ToNextAngleY;
    public float ToPrevAngleY;
    public Vector3 NextPos;
    public Vector3 PrevPos;
    public float NextGap;
    public float PrevGap;


    // Start is called before the first frame update
    void Start()
    {
        PrevCP = Prev.GetComponentInChildren<CheckPoint>();
        NextCP = Next.GetComponentInChildren<CheckPoint>();

        ToNext = NextCP.transform.position - transform.position;
        ToPrev = PrevCP.transform.position - transform.position;
        ToNext.y = 0;
        ToPrev.y = 0;

        NextGap = Vector3.Distance(NextCP.transform.position, transform.position);
        PrevGap = Vector3.Distance(PrevCP.transform.position, transform.position);

        NextPos = NextCP.transform.position;
        PrevPos = PrevCP.transform.position;

        ToNextAngleY = Vector3.Angle(transform.position, NextPos);
        ToPrevAngleY = Vector3.Angle(transform.position, PrevPos);

        Debug.Log(ToNext);
        Debug.Log(ToPrev);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
