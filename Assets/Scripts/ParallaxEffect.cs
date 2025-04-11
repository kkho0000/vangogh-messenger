using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] layers; // �洢��ͬͼ���Transform����
    public float[] parallaxScales; // ÿ��ͼ����Ӳ��������ӣ�ֵԽ���ƶ�Խ��

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
            // ����������ƶ�����
            Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

            // �����Ӳ����������ƶ�ͼ��
            layers[i].position += deltaMovement * parallaxScales[i];
        }

        // ������һ֡�����λ��
        previousCameraPosition = cameraTransform.position;
    }
}