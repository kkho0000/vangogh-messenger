using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;          // 玩家角色的Transform（跟随目标）
    public Transform farBackground;   // 远景背景（移动速度 = 100% 相机速度）
    public Transform middleBackground; // 中景背景（移动速度 = 50% 相机速度，实现视差效果）
    private Vector2 lastPos;          // 上一帧相机的位置（用于计算移动距离）

    void Start()
    {
        lastPos = transform.position; // 初始化记录相机起始位置
    }

    void Update()
    {
        // 让相机跟随玩家（X和Y轴），但Z轴保持不变（保持相机深度）
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 计算相机这一帧移动了多远（当前帧位置 - 上一帧位置）
        Vector2 amountToMove = new Vector2(
            transform.position.x - lastPos.x,
            transform.position.y - lastPos.y
        );

        // 移动远景背景（1:1 跟随相机）
        farBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f);

        // 移动中景背景（0.5倍速度，制造视差效果）
        middleBackground.position += new Vector3(amountToMove.x * 0.5f, amountToMove.y * 0.5f, 0f);

        // 更新“上一帧位置”，供下一帧计算使用
        lastPos = transform.position;
    }
}