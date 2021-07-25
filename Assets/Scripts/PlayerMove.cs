using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speed = 5;
    //�κ��丮
    private Inventory inventory;

    [SerializeField] private UI_Inventory uiInventory;

    
    private void Awake()
    {
        //�κ��丮
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
    [ �˰��� : ������� �Է��� �޾� �̵� ]

    1. ������� �Է��� ����

      �¿�h GetAxis : [��] = -1 / [��] = 1
      ����v GetAxis : [��] = -1 / [��] = 1

    2. ��������

     Vector: 
      right = (1, 0, 0) / left = (-1, 0, 0)
         up = (0, 1, 0) / down = (0, -1, 0)

      dir = (1,0,0)h +(0,1,0)�� �Ǹ� �Է� �������� 4������ ������ �� �ְ� ��
       ����, Vector3 dir = Vector3.right * h + Vector3.up * v;
                         = new Vector3(h, v, 0);
  
    3. �̵�

    : �̵���ɾ�transform.�Է°��������ض� �� �ӷ�(dir x speed)�� deltaTime�� ����ϵ���

      transform.position = tansform.position + dir * speed * Time.deltaTime;
      transform.position += dir * speed * Time.deltaTime;
 
      �̵����� P = P0 + vt / �̵���ġ = ������ġ + �ӵ�x�ð�      : t = deltaTime
      �ӵ����� v = v0 + at / ����ӷ� = �����ӷ� + ���ӵ�x�ð�    : Rigidbody ����
            �� F = ma      /       �� = ���� x ���ӵ�             : Rigidbody ����

      
 */