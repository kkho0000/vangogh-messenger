using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public float upwardForce = 10f; // 向上的力大小

    // 当触发器被触发时调用
    private void OnTriggerStay(Collider other)
    {
        // 检查碰撞物体是否有 "Player" 标签
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // 给 Player 添加一个向上的力
                playerRb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
            }
        }
    }
}