using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speed = 5;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
     
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, v, 0);

        if(h != 0 || v != 0 )
        {
            anim.SetBool("isMove", true);
        }
        else
            anim.SetBool("isMove", false);

        anim.SetFloat("InputX", h);
        anim.SetFloat("InputY", v);

        transform.Translate(dir * speed * Time.deltaTime);
    }
}