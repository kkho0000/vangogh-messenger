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
    private bool isGrounded; // 判断是否在地面上

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
        CheckGrounded(); // 检查是否在地面上

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        speed = Mathf.Abs(hor * moveSpeed); // 计算速度
        animator.SetFloat("speed", speed); // 设置动画参数
        if (hor != 0)
        {
            rb.velocity = new Vector3(hor * moveSpeed, 0, rb.velocity.y); // 控制左右和前后移动
            transform.localScale = new Vector3(0.3f, 0.3f, hor > 0 ? 0.3f : -0.3f); // 控制角色朝向
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // 检测跳跃且在地面上
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse); // 添加跳跃力
            isGrounded = false; // 跳跃后不在地面上
        }
    }

    // 检查是否在地面上
    void CheckGrounded()
    {
        float rayLength = 0.1f; // 射线长度
        RaycastHit hit;
        // 从玩家位置向下发射射线
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}