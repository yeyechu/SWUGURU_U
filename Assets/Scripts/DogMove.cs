/*using System.Collections;
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
    Collider2D collider2D;
    float distanceToPlayer;
    bool isTouching = false;

    public GameObject doghouse;

    public class Dog
    {
        public enum DogState
        {
            Idle,
            Move,
            GetSnack
        }
    }

    public Dog.DogState dogState;

    // Start is called before the first frame update
    void Start()
    {
        retVector = transform.position;
        dogState = Dog.DogState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (dogState)
        {
            case Dog.DogState.Idle:
                Idle();
                break;

            case Dog.DogState.Move:
                Move();
                break;

            case Dog.DogState.GetSnack:
                GetSnack();
                break;
        }
    }

    void Idle()
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

    void Move()
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

    void GetSnack()
    {
        float disTodoghouse = Vector3.Distance(transform.position, doghouse.transform.position);

        if (disToDoghouse < 0.1)
        {
            Vector3 dirToDoghouse = doghouse.transform.position - transform.position;
            dirToDoghouse.Normalize();
            transform.position += dirToDoghouse * speed * Time.deltaTime;
        }
        else
        {
            transform.position = doghouse.transform.position;
            dogState = Dog.DogState.Idle;
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
        yield return new WaitForSeconds(1f);
    }
}*/
