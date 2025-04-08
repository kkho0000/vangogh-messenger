using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpSpeed; // 跳跃速度
    public float moveSpeed; // 移动速度
    private float speed;
    private GameObject paint; // 线段数组
    Rigidbody rb;
    Animator animator; // 动画控制器
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 获取刚体组件
        animator = GetComponent<Animator>(); // 获取动画控制器组件
        paint = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        speed = Mathf.Abs(hor * moveSpeed); // 计算速度
        animator.SetFloat("speed", speed); // 设置动画参数
        if (hor != 0)
        {
            rb.velocity = new Vector3(hor * moveSpeed, 0, rb.velocity.y); // 控制左右和前后移动
            transform.localScale = new Vector3(0.3f, 0.3f, hor > 0 ? 0.3f : -0.3f); // 控制角色朝向
        }
        if (Input.GetKeyDown(KeyCode.Space)) // 检测跳跃
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse); // 添加跳跃力
        }
    }
}
