using UnityEngine;
using System.Collections;

public class LineObjectUIManager : MonoBehaviour
{
    // ����ʾ�� GameObject
    public GameObject showGameObject;
    public GameObject showGameObject1;
    // �����ص� GameObject
    public GameObject hideGameObject;
    // ���Ҫ��ʾ�� GameObject
    public GameObject finalGameObject;

    // ����Ƿ��Ѿ������ʾ����
    private bool hasDisplayProcessCompleted = false;
    private Transform playerTransform;

    // ���� targetController �ű�
    public TargetController targetController;

    private void Update()
    {
        if (targetController.uiClick && !hasDisplayProcessCompleted)
        {
            hasDisplayProcessCompleted = true;
            playerTransform = GameObject.FindWithTag("Player").transform;
            SetGameObjectPosition(showGameObject);
            StartCoroutine(HandleGameObjectChanges());
        }
        if (!targetController.uiClick)
        {
            hideGameObject.SetActive(true);
            showGameObject1.SetActive(false);
            finalGameObject.SetActive(false);
        }
        if (targetController.uiClick && hasDisplayProcessCompleted)
        {
            hideGameObject.SetActive(false);
            showGameObject1.SetActive(true);
        }

    }

    private void SetGameObjectPosition(GameObject go)
    {
        if (playerTransform != null)
        {
            Vector3 position = playerTransform.position;
            // ��������ƫ�� 1 ����λ�����Ը���ʵ���������
            position.y -= 1f;
            position.z = 0;
            go.transform.position = position;
        }
    }

    private IEnumerator HandleGameObjectChanges()
    {
        // ��������ʾ�� GameObject
        hideGameObject.SetActive(false);
        // ��ʾ�µ� GameObject

        showGameObject.SetActive(true);
        showGameObject1.SetActive(true);

        // �ȴ� 1 ��
        yield return new WaitForSeconds(1f);

        // ��������ʾ�� GameObject
        showGameObject.SetActive(false);
        // ��ʾ���յ� GameObject
        SetGameObjectPosition(finalGameObject);
        finalGameObject.SetActive(true);
    }
}
