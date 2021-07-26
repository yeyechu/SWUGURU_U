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
    float distanceToPlayer;
    public float barkRange = 1;

    enum DogState
    {
        Idle,
        Move,
        Find
    }

    DogState dogState;

    // Start is called before the first frame update
    void Start()
    {
        dogState = DogState.Idle;
        retVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (dogState)
        {
            case DogState.Idle:
                Idle();
                break;

            case DogState.Move:
                Move();
                break;

            case DogState.Find:
                Find();
                break;
        }

    }

    void Idle()
    {
        if (distanceToPlayer <= barkRange)
        {
            dogState = DogState.Find;
        }
    }

    void Move()
    {
        degree += speed;
        float radian = degree * Mathf.PI / 180;

        retVector.x += rangeX * Mathf.Cos(radian);
        retVector.y += rangeY * Mathf.Sin(radian);

        transform.position = retVector;

        if (distanceToPlayer <= barkRange)
        {
            dogState = DogState.Find;
        }
    }

    void Find()
    {
        print("¸Û¸Û");
        dogState = DogState.Idle;
    }

}
