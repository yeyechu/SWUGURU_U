using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfRoom : MonoBehaviour
{
    private GameObject player;
    


    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //검은 화면으로 변하는 애니메이션
        print("충돌");
        StartCoroutine(Waiting());
        Camera.main.transform.position = new Vector3(-15, 1, -15);
        player.transform.position = new Vector3(-6, -1, -1);
        StartCoroutine(Waiting());
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.3f);
    }
}
