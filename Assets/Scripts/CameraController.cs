using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;          // ��ҽ�ɫ��Transform������Ŀ�꣩
    public Transform farBackground;   // Զ���������ƶ��ٶ� = 100% ����ٶȣ�
    public Transform middleBackground; // �о��������ƶ��ٶ� = 50% ����ٶȣ�ʵ���Ӳ�Ч����
    private Vector2 lastPos;          // ��һ֡�����λ�ã����ڼ����ƶ����룩

    void Start()
    {
        lastPos = transform.position; // ��ʼ����¼�����ʼλ��
    }

    void Update()
    {
        // �����������ң�X��Y�ᣩ����Z�ᱣ�ֲ��䣨���������ȣ�
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // ���������һ֡�ƶ��˶�Զ����ǰ֡λ�� - ��һ֡λ�ã�
        Vector2 amountToMove = new Vector2(
            transform.position.x - lastPos.x,
            transform.position.y - lastPos.y
        );

        // �ƶ�Զ��������1:1 ���������
        farBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f);

        // �ƶ��о�������0.5���ٶȣ������Ӳ�Ч����
        middleBackground.position += new Vector3(amountToMove.x * 0.5f, amountToMove.y * 0.5f, 0f);

        // ���¡���һ֡λ�á�������һ֡����ʹ��
        lastPos = transform.position;
    }
}