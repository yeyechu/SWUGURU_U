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
    Collider2D collider2D;
    float distanceToPlayer;
    bool isTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        retVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        isTouching = true;
        if (distanceToPlayer > 5)
        {
            Debug.Log("¹þ¾î³²");
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
}
