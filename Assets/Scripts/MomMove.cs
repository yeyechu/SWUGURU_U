using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomMove : MonoBehaviour
{
    enum MomState
    {
        Idle,
        Move,
        Detect
    }

    MomState momState;

    GameObject player;
    public Transform Target;

    public float findDistance = 5f;

    public float moveSpeed = 5;

    float currentTime = 0;

    public float MoveTime = 10f;

    float DetectTime = 100f;



    // Start is called before the first frame update
    void Start()
    {
        momState = MomState.Idle;

        //플레이어 
        player = GameObject.Find("Player");


    }

    // Update is called once per frame
    void Update()
    {
        switch (momState)
        {
            case MomState.Idle:
                Idle();
                break;

            case MomState.Move:
                Move();
                break;

            case MomState.Detect:
                Detect();
                break;

        }
    }

    void Idle()
    {
        if (currentTime < MoveTime)
        {
            momState = MomState.Move;
            print("Idle -> move");
            currentTime = 0;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    void Move()
    {

        if (Vector3.Distance(player.transform.position, transform.position) <= findDistance)
        {
            momState = MomState.Detect;
            print("무브에서 디텍트");
        }

        if (Vector3.Distance(player.transform.position, transform.position) > findDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, moveSpeed * Time.deltaTime);


        }
    }

    void Detect()
    {
        print("하트 하나 감소");
        momState = MomState.Idle;
        print("무브에서 아이들");
        player.transform.position = new Vector3(0, 0, 0);
        transform.position = new Vector3(-33, 3, 0);
        //하트감소UI
    }


}