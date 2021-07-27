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

    // Start is called before the first frame update
    void Start()
    {
        retVector = transform.position;
        collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        print("港港");
        StartCoroutine(Bark());
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > 5)
        {
            return;
        }
    }

    IEnumerator Bark()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("内风凭 角青凳");
    }

    void Move()
    {
        degree += speed;
        float radian = degree * Mathf.PI / 180;

        retVector.x += rangeX * Mathf.Cos(radian);
        retVector.y += rangeY * Mathf.Sin(radian);

        transform.position = retVector;
    }
}
