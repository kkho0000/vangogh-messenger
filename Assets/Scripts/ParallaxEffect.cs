using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] layers; // 存储不同图层的Transform数组
    public float[] parallaxScales; // 每个图层的视差缩放因子，值越大，移动越慢

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            // 计算相机的移动距离
            Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

            // 根据视差缩放因子移动图层
            layers[i].position += deltaMovement * parallaxScales[i];
        }

        // 更新上一帧相机的位置
        previousCameraPosition = cameraTransform.position;
    }
}