using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : MonoBehaviour
{
    GameObject point;
    public float distanceToPoint;
    public float successRange = 10f;

    public float t = 1f; // speed
    public float l = 10f; // length from 0 to endpoint.
    public float posX = 1f; // Offset


    // Start is called before the first frame update
    void Start()
    {
        point = GameObject.Find("Point");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(posX + Mathf.PingPong(t * Time.time, l), 4, 0);
        transform.position = pos;

        if (Input.GetButtonDown("Jump"))
        {
            distanceToPoint = Vector3.Distance(transform.position, point.transform.position);

            if(distanceToPoint <= successRange)
            {
                print("성공");
            }
            else
            {
                print("실패");
            }
        }
    }
}
