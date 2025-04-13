using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetData : MonoBehaviour
{
    private bool isTrigger = false; // 是否是触发器
    private GameObject targetLine = null; // 线段物体
    public TargetController targetController;
    public void LineUp(GameObject line)
    {
        isTrigger = true; // 设置为触发器
        if (line)
        {
            targetLine = line; // 设置目标线段
        }
    }

    public void Reset()
    {
        isTrigger = false; // 重置为非触发器
        if (targetLine != null)
        {
            Destroy(targetLine); // 销毁目标线段
            targetLine = null; // 清空目标线段引用
        }
        targetController.uiClick=false;
    } 

    public bool IsTrigger()
    {
        return isTrigger; // 返回是否是触发器
    }
}
