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

    public Transform Target1;
    public Transform Target2;

    public float findDistance = 2f;

    public float moveSpeed = 10;

    float currentTime = 0;

    public float MoveTime = 0.2f;




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
        currentTime += Time.deltaTime;

        if (currentTime >= MoveTime)
        {
            momState = MomState.Move;
            print("Idle -> move");
            currentTime = 0;
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target1.transform.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(player.transform.position, transform.position) <= findDistance)
        {

            momState = MomState.Detect;
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Target2.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    void Detect()
    {
        StartCoroutine(Waiting());
        //�ִϸ��̼�
        momState = MomState.Idle;
        print("���꿡�� ���̵�");
        player.transform.position = new Vector3(0, 0, 0);
        transform.position = Target2.transform.position;
        //��Ʈ����UI
        print("��Ʈ �ϳ� ����");
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.1f);
    }

}