using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speed = 5;
    //인벤토리
    private Inventory inventory;

    [SerializeField] private UI_Inventory uiInventory;

    
    private void Awake()
    {
        //인벤토리
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, v, 0);

        transform.Translate(dir * speed * Time.deltaTime);
    }
}

/*
    [ 알고리즘 : 사용자의 입력을 받아 이동 ]

    1. 사용자의 입력을 받음

      좌우h GetAxis : [←] = -1 / [→] = 1
      상하v GetAxis : [↓] = -1 / [↑] = 1

    2. 방향지정

     Vector: 
      right = (1, 0, 0) / left = (-1, 0, 0)
         up = (0, 1, 0) / down = (0, -1, 0)

      dir = (1,0,0)h +(0,1,0)가 되면 입력 값에따라 4방으로 움직일 수 있게 됨
       →즉, Vector3 dir = Vector3.right * h + Vector3.up * v;
                         = new Vector3(h, v, 0);
  
    3. 이동

    : 이동명령어transform.입력값을번역해라 → 속력(dir x speed)을 deltaTime에 비례하도록

      transform.position = tansform.position + dir * speed * Time.deltaTime;
      transform.position += dir * speed * Time.deltaTime;
 
      이동공식 P = P0 + vt / 이동위치 = 원래위치 + 속도x시간      : t = deltaTime
      속도공식 v = v0 + at / 현재속력 = 원래속력 + 가속도x시간    : Rigidbody 적용
            힘 F = ma      /       힘 = 질량 x 가속도             : Rigidbody 적용

      
 */