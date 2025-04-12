using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject prefab; // 颜料预制体
    public Color originColor;
    private bool _uiClick=false;
    private GameObject Lines; // 线段数组
    private GameObject player; // 玩家物体
    private GameObject spawner; // 生成器物体
    private SphereCollider sphereCollider; // 球形碰撞器
    private bool isAvaliable = true; // 是否可用
    private bool isMouseOver = false; // 鼠标是否悬停在物体上

    void InstantiateLine(Vector3 start, Vector3 end)
    {

        // 实例化线段预制体，初始位置为起点
        GameObject line = Instantiate(prefab, start, Quaternion.identity);

        // 计算线段的方向和长度（基于世界坐标系）
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        // 设置线段的旋转（基于世界坐标系）
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // 使用 x 和 z 计算角度

        // 设置线段的缩放
        line.transform.localScale = new Vector3(line.transform.localScale.x, distance / 2, line.transform.localScale.z);

        // 设置线段的中心位置（基于世界坐标系）
        Vector3 center = start + direction / 2;
        line.transform.position = center;
        line.transform.rotation = Quaternion.LookRotation(direction);
        line.transform.Rotate(-90, 0, 0); // 调整为 y 轴对齐到方向
        line.transform.Rotate(0, 90, 0); // 调整为 x 轴对齐到方向

        // 设置线段数据
        LineData lineData = line.GetComponent<LineData>();
        // 计算变换后的 start 和 end 坐标
        lineData.startPoint = start; // 将局部坐标 start 转换为世界坐标
        lineData.endPoint = end; // 将局部坐标 end 转换为世界坐标
        lineData.lineColor = originColor; // 设置线段的颜色

        // 设置线段的父物体为线段数组
        line.transform.SetParent(Lines.transform);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 获取玩家物体
        Lines = GameObject.FindGameObjectWithTag("Lines"); // 获取线段数组物体
        sphereCollider = GetComponent<SphereCollider>(); // 获取球形碰撞器组件
        //spawner = transform.parent.gameObject; // 获取生成器物体
    }

    void Update()
    {
        // 使用射线检测鼠标是否指向当前物体
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject) // 如果射线命中当前物体
            {
                if (!isMouseOver) // 如果鼠标刚刚进入物体
                {
                    isMouseOver = true;
                    HandleMouseEnter(); // 调用鼠标进入逻辑
                }
            }
            else if (isMouseOver) // 如果鼠标离开物体
            {
                isMouseOver = false;
                HandleMouseExit(); // 调用鼠标离开逻辑
            }
        }
        else if (isMouseOver) // 如果射线未命中任何物体且鼠标之前在物体上
        {
            isMouseOver = false;
            HandleMouseExit(); // 调用鼠标离开逻辑
        }

        // 检测鼠标点击
        if (isMouseOver && Input.GetMouseButtonDown(0)) // 鼠标左键点击
        {
            
            HandleMouseDown(); // 调用鼠标点击逻辑
        }
    }

    private void HandleMouseEnter()
    {
        for (int i = 0; i < Lines.transform.childCount; i++)
        {
            GameObject line = Lines.transform.GetChild(i).gameObject;
            LineData lineData = line.GetComponent<LineData>();

            if (lineData.lineColor == originColor) // 如果颜色相同
            {
                continue; // 跳过
            }

            if (DoLinesIntersect(lineData.startPoint, lineData.endPoint, player.transform.position, transform.position))
            {
                isAvaliable = false; // 设置为不可用
                return;
            }
        }
    }

    private void HandleMouseExit()
    {
        isAvaliable = true; // 设置为可用
    }

    private void HandleMouseDown()
    {
        if (!isAvaliable) return; // 如果不可用则返回
        Vector3 trans = transform.position; // 获取当前物体位置
        Vector3 playerPos = player.transform.position; // 玩家位置
        InstantiateLine(playerPos + new Vector3(0, 1.4f, 0f), trans); // 实例化颜料
        sphereCollider.enabled = false; // 禁用球形碰撞器
        _uiClick = true;
    }
    public bool uiClick
    {
        get { return _uiClick; }
        set { _uiClick = value; }
    }

    // 检测两条线段是否相交
    private bool DoLinesIntersect(Vector3 p1, Vector3 p2, Vector3 q1, Vector3 q2)
    {
        float Cross(Vector3 a, Vector3 b) => a.x * b.z - a.z * b.x; // 使用 x 和 z 计算叉积

        Vector3 r = p2 - p1;
        Vector3 s = q2 - q1;
        float rxs = Cross(r, s);
        float qpxr = Cross(q1 - p1, r);

        if (Mathf.Approximately(rxs, 0f) && Mathf.Approximately(qpxr, 0f))
        {
            // 两条线段共线的情况
            return false;
        }

        if (Mathf.Approximately(rxs, 0f) && !Mathf.Approximately(qpxr, 0f))
        {
            // 两条线段平行且不共线
            return false;
        }

        float t = Cross(q1 - p1, s) / rxs;
        float u = Cross(q1 - p1, r) / rxs;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }
}