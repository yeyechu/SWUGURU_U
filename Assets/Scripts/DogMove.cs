using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMove : MonoBehaviour
{
    Vector3 retVector;
    public float speed = 1;
    private float degree = 0;
    public float rangeX = 0.1f;
    public float rangeY = 0.1f;

    public GameObject player;
    new Collider2D collider2D;
    float distanceToPlayer;
    bool isTouching = false;

    public GameObject doghouse;
    public enum DogState
    {
            Idle,
            Move,
            GetSnack
    }
  

    public static DogState dogState;

    // Start is called before the first frame update
    void Start()
    {
        retVector = transform.position;
        dogState = DogState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (dogState)
        {
            case DogState.Idle:
                Idle();
                break;

            case DogState.Move:
                Move();
                break;

            case DogState.GetSnack:
                GetSnack();
                break;
        }
    }

    public void Idle()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (isTouching)
        {
            if (distanceToPlayer <= 5)
            {
                return;
            }

            OnTriggerEnter2D(collider2D);
            return;
        }
    }

    public void Move()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (isTouching)
        {
            if (distanceToPlayer <= 5)
            {
                return;
            }

            //Debug.Log("Ãæµ¹°¨Áö");
            
            OnTriggerEnter2D(collider2D);
            return;
        }

        degree += speed;
        float radian = degree * Mathf.PI / 180;

        retVector.x += rangeX * Mathf.Cos(radian);
        retVector.y += rangeY * Mathf.Sin(radian);

        transform.position = retVector;
    }

    public void GetSnack()
    {
        float disToDoghouse = Vector3.Distance(transform.position, doghouse.transform.position);

        if (disToDoghouse < 0.1)
        {
            Vector3 dirToDoghouse = doghouse.transform.position - transform.position;
            dirToDoghouse.Normalize();
            transform.position += dirToDoghouse * speed * Time.deltaTime;
        }
        else
        {
            transform.position = doghouse.transform.position;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        isTouching = true;
        if (distanceToPlayer > 5)
        {
            //Debug.Log("¹þ¾î³²");
            isTouching = false;
            return;
        }
        StartCoroutine(Bark());
    }

    IEnumerator Bark()
    {
        print("¸Û¸Û");
        yield return new WaitForSeconds(1.0f);
    }
}
