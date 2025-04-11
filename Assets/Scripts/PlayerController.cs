using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpSpeed; // 跳跃速度
    public float moveSpeed; // 
    public float ropeSpeed; // 线段速度
    public GameObject letter;
    private float speed;
    private Vector3 startPoint; // 线段起点
    private Vector3 endPoint; // 线段终点
    private Vector3 offset = new Vector3(0, 1.4f, 0);
    private bool isHolding = false;
    private bool isGround; // 判断是否在地面上
    private bool finish = false; // 判断是否完成
    Rigidbody rb;
    Animator animator; // 动画控制器

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 获取刚体组件
        animator = GetComponent<Animator>(); // 获取动画控制器组件
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        speed = hor * moveSpeed; // 计算速度
        animator.SetFloat("speed", Mathf.Abs(speed)); // 设置动画参数
        if (hor != 0)
        {
            if (isHolding)// 如果有持有状态
            {
                if ((startPoint.x - endPoint.x) * hor < 0) // 如果起点在终点的右侧
                {
                    transform.position = Vector3.MoveTowards(transform.position, endPoint - offset, ropeSpeed * Time.deltaTime); // 控制角色朝向
                }
                else // 如果起点在终点的左侧
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPoint - offset, ropeSpeed * Time.deltaTime); // 控制角色朝向
                }
            }
            else
            {
                rb.velocity = new Vector3(speed, rb.velocity.y, 0); // 控制左右和前后移动
            }
            transform.localScale = new Vector3(1f, 1f, hor > 0 ? 1f : -1f); // 控制角色朝向
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround) // 检测跳跃
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse); // 添加跳跃力
            animator.SetTrigger("jump"); // 设置跳跃动画触发器
            isGround = false; // 设置在地面上为 false

            // 分离移动和跳跃逻辑，先计算水平速度

            // 统一设置刚体速度


            speed = Mathf.Abs(hor * moveSpeed);
            animator.SetFloat("speed", speed);
            if (hor != 0)
            {
                transform.localScale = new Vector3(0.3f, 0.3f, hor > 0 ? 0.3f : -0.3f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGround = true; // 检测是否在地面上
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检测是否是 "Player" 碰撞到 "Lines" 的子物体
        if (other.CompareTag("Finish") && !finish)
        {
            // 触发完成动画
            finish = true; // 设置完成状态为 true
            animator.SetTrigger("mail"); // 设置完成动画触发器
            // 开始协程逐渐移动到目标位置
            StartCoroutine(MoveToTarget(other.transform.position + new Vector3(-0.5f, -0.4f, 0)));
            // 延迟 4 秒销毁子物体
            if (letter != null)
            {
                Destroy(letter, 4f); // 4 秒后销毁子物体
            }
            rb.isKinematic = true;
            this.enabled = false;
        }
    }

    // 检测触发器
    private void OnTriggerStay(Collider other)
    {
        // 检测是否是 "Player" 碰撞到 "Lines" 的子物体
        if (other.CompareTag("Lines"))
        {
            LineData lineData = other.GetComponent<LineData>();
            // 如果按下 Shift 键，将角色位置重置到线段的中心
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (lineData != null)
                { 
                    if (!isHolding) // 如果没有持有状态
                    {
                        startPoint = lineData.startPoint;
                        endPoint = lineData.endPoint;

                        // 计算角色到线段的最近点
                        Vector3 nearestPoint = GetNearestPointOnLine(startPoint, endPoint, transform.position);
                        transform.position = nearestPoint - offset; // 设置角色位置为最近点
                        animator.SetTrigger("hold"); // 设置持有动画触发器
                        isHolding = true; // 设置持有状态为 true
                        rb.velocity = Vector3.zero; // 停止角色的运动
                        rb.isKinematic = true; // 设置刚体为运动学
                    }
                }
            }
            else if (isHolding)
            {
                isHolding = false; // 设置持有状态为 false
                animator.SetTrigger("release"); // 设置释放动画触发器
                rb.isKinematic = false; // 设置刚体为非运动学
            }
        }
    }


    // 计算点到线段的最近点
    private Vector3 GetNearestPointOnLine(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 lineDirection = end - start; // 线段方向
        float lineLength = lineDirection.magnitude; // 线段长度
        lineDirection.Normalize(); // 单位化线段方向

        // 计算点到线段起点的向量
        Vector3 pointToStart = point - start;

        // 计算点在线段方向上的投影长度
        float projectionLength = Vector3.Dot(pointToStart, lineDirection);

        // 限制投影长度在 [0, lineLength] 范围内
        projectionLength = Mathf.Clamp(projectionLength, 0, lineLength);

        // 计算最近点的位置
        return start + lineDirection * projectionLength;
    }

    // 协程：逐渐移动到目标位置
    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        float duration = 2f; // 移动持续时间
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            // 逐渐移动到目标位置
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        // 确保最终位置精确到目标位置
        transform.position = targetPosition;
    }
}
