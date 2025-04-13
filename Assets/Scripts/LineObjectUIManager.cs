using UnityEngine;
using System.Collections;

public class LineObjectUIManager : MonoBehaviour
{
    // 待显示的 GameObject
    public GameObject showGameObject;
    public GameObject showGameObject1;
    // 待隐藏的 GameObject
    public GameObject hideGameObject;
    // 最后要显示的 GameObject
    public GameObject finalGameObject;

    // 标记是否已经完成显示流程
    private bool hasDisplayProcessCompleted = false;
    private Transform playerTransform;

    // 引用 targetController 脚本
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
            // 假设向下偏移 1 个单位，可以根据实际情况调整
            position.y -= 1f;
            position.z = 0;
            go.transform.position = position;
        }
    }

    private IEnumerator HandleGameObjectChanges()
    {
        // 隐藏已显示的 GameObject
        hideGameObject.SetActive(false);
        // 显示新的 GameObject

        showGameObject.SetActive(true);
        showGameObject1.SetActive(true);

        // 等待 1 秒
        yield return new WaitForSeconds(1f);

        // 隐藏新显示的 GameObject
        showGameObject.SetActive(false);
        // 显示最终的 GameObject
        SetGameObjectPosition(finalGameObject);
        finalGameObject.SetActive(true);
    }
}
