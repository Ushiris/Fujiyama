using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CheckPoint CheckPos { get; set; }
    public Vector3 CpWorldPos { get; set; }
    public float speed = 1;
    public float JumpFouce;
    public GameObject Director;
    PathDirector pathDirector;

    float path;
    float DefaultSpeed;
    bool IsGround = false;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        path = 0.1f;
        DefaultSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        pathDirector = Director.GetComponent<PathDirector>();
        CheckPos = pathDirector.StartPoint;
        CpWorldPos = CheckPos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //path
        if(Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow))
        {
            if (pathDirector.path_max < path + speed * Time.deltaTime)
            {
                path = pathDirector.path_max - 0.1f;
            }
            else
            {
                path += speed * Time.deltaTime;
            }
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (path - speed * Time.deltaTime <= 0)
            {
                path = 0.1f;
            }
            else
            {
                path -= speed * Time.deltaTime;
            }
        }

        //jump
        if(Input.GetKeyDown(KeyCode.Space) && IsGround)
        {
            rb.AddForce(new Vector3(0, JumpFouce));
            speed /= 2;
        }

        //move
        Debug.Log(path);
        if (path < CheckPos.path)
        {
            float par = CheckPos.prev_gap / (path-(CheckPos.path-CheckPos.prev_gap));
            Vector3 NewPosition = new Vector3(CpWorldPos.x+ CheckPos.Prev.x*par, transform.position.y, CpWorldPos.z + CheckPos.Prev.z*par);
            transform.rotation.Set(0, CheckPos.Prev.y, 0, transform.rotation.z);
            transform.position = NewPosition;
        }
        else if (path > CheckPos.path)
        {
            float par = CheckPos.next_gap / (path-CheckPos.path);
            Vector3 NewPosition = new Vector3(CpWorldPos.x + CheckPos.Next.x*par, transform.position.y, CpWorldPos.z + CheckPos.Next.z*par);
            transform.rotation.Set(0, CheckPos.Next.y, 0, transform.rotation.z);
            transform.position = NewPosition;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag=="Plane" && !IsGround)
        {
            IsGround = true;
            speed = DefaultSpeed;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            IsGround = false;
        }
    }
}
