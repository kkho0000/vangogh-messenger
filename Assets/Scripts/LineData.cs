using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineData : MonoBehaviour
{
    public Vector3 startPoint; // 线段的起点
    public Vector3 endPoint;   // 线段的终点
    public Color lineColor;    // 线段的颜色
    public float lineWidth;    // 线段的宽度
    private GameObject player; // 玩家物体
    private bool onHold = false; // 是否被持有
    private PlayerController playerController; // 玩家控制器

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>(); // 获取玩家控制器组件
    }

    public bool IsOnHold()
    {
        return onHold; // 返回是否被持有
    }

    public void SetOnHold()
    {
        onHold = true; // 设置是否被持有
    }

    public void Release()
    {
        onHold = false; // 释放线段
    }

    private void OnDisable()
    {
        if (onHold) // 如果线段被持有
        {
            playerController.ResetHoldingState(); // 重置玩家的持有状态
        }
    }
}
