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

            if (i == 0)
            {
                prev = cp;
                next = stages[i + 1].GetComponentInChildren<CheckPoint>();
                cp_pos = cp.transform.position;
                next_pos = next.transform.position;
                pd.StartPoint = cp;
            }
            else if (i == stages.Count - 1)
            {
                prev = stages[i - 1].GetComponentInChildren<CheckPoint>();
                cp_pos = cp.transform.position;
                next_pos = pd.EndObject.transform.position;
                prev.next_gap = cp.path - prev.path;
            }
            else
            {
                prev = stages[i - 1].GetComponentInChildren<CheckPoint>();
                next = stages[i + 1].GetComponentInChildren<CheckPoint>();
                cp_pos = cp.transform.position;
                next_pos = next.transform.position;
                prev.next_gap = cp.path - prev.path;
            }

            cp.prev_gap = cp.path - prev.path;
            prev_pos = prev.transform.position;
            cp.Prev = prev_pos - cp_pos;
            cp.Next = next_pos - cp_pos;
            cp.path = path_length;
            path_length += Vector3.Distance(cp_pos, next_pos);
        }

        pd.path_max = path_length;

        foreach(var a in stages)
        {
            Debug.Log(a.GetComponentInChildren<CheckPoint>().Prev);
            Debug.Log(a.GetComponentInChildren<CheckPoint>().Next);
            Debug.Log(a.GetComponentInChildren<CheckPoint>().path);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
