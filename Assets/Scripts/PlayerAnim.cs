using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public string upAnime = "";     // �� ���� ��Inspector�� ����
    public string downAnime = "";   // �Ʒ� ���⣺Inspector�� ����
    public string rightAnime = "";  // ������ ���⣺Inspector�� ����
    public string leftAnime = "";   // ���� ���⣺Inspector�� ����
    public string IdleAnime = "";

    string nowMode = "";
    string oldMode = "";

    void Start()// ó���� �����Ѵ� 
    {
        nowMode = IdleAnime;
        oldMode = "";
    }

    void Update()// ��� �����Ѵ� 
    {
        if (Input.GetKey("up"))// �� Ű��
        {
            nowMode = upAnime;
        }
        if (Input.GetKey("down"))// �Ʒ� Ű��
        {
            nowMode = downAnime;
        }
        if (Input.GetKey("right"))// ������ Ű��
        {
            nowMode = rightAnime;
        }
        if (Input.GetKey("left"))// ���� Ű��
        {
            nowMode = leftAnime;
        }
    }
   
    void FixedUpdate() // ��� �����Ѵ�(���� �ð�����)
    {
        // ���� �ٸ� Ű�� ������ 
        if (nowMode != oldMode)
        {
            oldMode = nowMode;
            // �ִϸ��̼��� ��ȯ�Ѵ� 
            Animator animator = this.GetComponent<Animator>();
            animator.Play(nowMode);
        }
    }
}
