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

    public float findDistance = 5f;

    public float moveSpeed = 5;

    float currentTime = 0;

    public float DetectDelayTime = 5f;



    // Start is called before the first frame update
    void Start()
    {
        momState = MomState.Idle;

        //�÷��̾� 
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        switch(momState)
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
        if (currentTime < DetectDelayTime)
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
            print("���꿡�� ����Ʈ");
        }

        if (Vector3.Distance(player.transform .position, transform.position) > findDistance)
        {

            transform.position = new Vector3(10, 0, 0);
            
            
        }
    }

    void Detect()
    {
        print("��Ʈ �ϳ� ����");
        momState = MomState.Idle;
        print("���꿡�� ���̵�");
        player.transform.position = new Vector3(0, 0, 0);
        transform.position = new Vector3(-33, 3, 0);
        //��Ʈ����UI
    }

    
}
