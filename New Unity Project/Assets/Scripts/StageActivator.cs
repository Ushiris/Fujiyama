using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActivator : MonoBehaviour
{
    public List<GameObject> stages;
    public GameObject director;

    // Start is called before the first frame update
    void Start()
    {
        float path_length = 0f;
        PathDirector pd = director.GetComponent<PathDirector>();

        //construct stages
        for (int i = 0; i < stages.Count; i++)
        {
            CheckPoint cp = stages[i].GetComponentInChildren<CheckPoint>();
            CheckPoint next;
            CheckPoint prev;
            Vector3 cp_pos;
            Vector3 next_pos;
            Vector3 prev_pos;

            cp.path = path_length;

            if (i == 0)
            {
                prev = cp;
                next = stages[i + 1].GetComponentInChildren<CheckPoint>();
                cp_pos = cp.transform.position;
                next_pos = next.transform.position;
                pd.StartPoint = cp;
                cp.Prev = cp.transform;
            }
            else if (i == stages.Count - 1)
            {
                prev = stages[i - 1].GetComponentInChildren<CheckPoint>();
                cp_pos = cp.transform.position;
                next_pos = pd.EndObject.transform.position;
                prev.next_gap = cp.path - prev.path;
                cp.next_gap = Vector3.Distance(next_pos, cp_pos);
                prev.Next = cp.transform;
                cp.Prev = cp.transform;
                cp.Next = pd.EndObject.transform;
            }
            else
            {
                prev = stages[i - 1].GetComponentInChildren<CheckPoint>();
                next = stages[i + 1].GetComponentInChildren<CheckPoint>();
                cp_pos = cp.transform.position;
                next_pos = next.transform.position;
                prev.next_gap = cp.path - prev.path;
                cp.Prev = prev.transform;
                prev.Next = cp.transform;
            }

            cp.prev_gap = cp.path - prev.path;
            prev_pos = prev.transform.position;
            cp.PrevAngle = prev_pos - cp_pos;
            cp.NextAngle = next_pos - cp_pos;
            path_length += Vector3.Distance(cp_pos, next_pos);
        }

        pd.path_max = path_length;

        foreach(var a in stages)
        {
            Debug.Log(a.GetComponentInChildren<CheckPoint>().PrevAngle);
            Debug.Log(a.GetComponentInChildren<CheckPoint>().NextAngle);
            Debug.Log(a.GetComponentInChildren<CheckPoint>().path);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
