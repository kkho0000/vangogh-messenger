using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static SavePoint;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

[ExecuteInEditMode]
public class PlayerCamera : MonoBehaviour
{
    
    public GameObject[] targetObjects; // 要切换渲染效果的目标物体数组
    public Material[] blackAndWhiteMaterials; // 黑白材质数组
    private Material[][] originalMaterials; // 原始材质数组，二维数组以兼容不同渲染器
    public bool isBlackAndWhite = false;
    public bool isDie = false;
    public Image blackScreen;
    public CinemachineVirtualCamera virtualCamera;
    Animator animator;
    public GameObject confiner;
    public GameObject settlementUI; // 结算 UI 界面的 GameObject
    private int _collisionCount = 0; // 记录碰撞次数
    private bool interactionHandled = false;
    public void Die()
    {
        transform.position = GameManager.Instance.GetLastSavePosition(); 
        Debug.Log("Player respawned at the last save point.");

        // 获取 PlayerController 组件并重置状态
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.ResetPlayerState();
            // 重置其他状态
            isBlackAndWhite = false;
            ToggleRender(); // 重置渲染效果

        }
    }

    private void Start()
    {
        virtualCamera.m_Lens.OrthographicSize = 5.3f;
        Debug.Log("Player entered the save point.");
        GameManager.Instance.SetLastSavePosition(transform.position);
        animator = GetComponent<Animator>();
        originalMaterials = new Material[targetObjects.Length][];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            SpriteRenderer spriteRenderer = targetObjects[i].GetComponent<SpriteRenderer>();
            MeshRenderer meshRenderer = targetObjects[i].GetComponent<MeshRenderer>();

            if (spriteRenderer != null)
            {
                originalMaterials[i] = new Material[1] { spriteRenderer.sharedMaterial };
            }
            else if (meshRenderer != null)
            {
                originalMaterials[i] = meshRenderer.sharedMaterials;
            }
        }
    }

    private void Update()
    {
        if (isDie)
        {
            Die();
            isDie = false;
        }

    }
    public int collisionCount { 
        get { return _collisionCount; } 
        set { _collisionCount = value; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enermy"))
        {
            if (!interactionHandled)
            {
                Debug.Log("触发事件: 与 " + other.gameObject.name + " 触发");
                interactionHandled = true;
                _collisionCount++;
                Debug.Log(collisionCount);
                if (_collisionCount == 1)
                {
                    isBlackAndWhite = false;
                    ToggleRender();
                }
                else if (_collisionCount == 2)
                {
                    isBlackAndWhite = true;
                    ToggleRender();
                    Die();
                    //player.ReturnToSpawnPoint();
                    _collisionCount = 0; // 重置碰撞次数
                }
                


            }
        }
        if (other.CompareTag("End")) // 假设终点标签为 "End"
        {
            StartCoroutine(TriggerEndScene());
            Destroy(other.gameObject);
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (!interactionHandled)
        {
            // 在这里添加碰撞时要执行的逻辑
            //Debug.Log("碰撞事件: 与 " + collision.gameObject.name + " 碰撞");
            interactionHandled = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        interactionHandled = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        interactionHandled = false;
    }
    public void ToggleRender()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            SpriteRenderer spriteRenderer = targetObjects[i].GetComponent<SpriteRenderer>();
            MeshRenderer meshRenderer = targetObjects[i].GetComponent<MeshRenderer>();

            if (spriteRenderer != null)
            {
                if (isBlackAndWhite)
                {
                    spriteRenderer.sharedMaterial = originalMaterials[i][0];
                }
                else
                {
                    spriteRenderer.sharedMaterial = blackAndWhiteMaterials[i];
                }
            }
            else if (meshRenderer != null)
            {
                Material[] materialsToApply = new Material[meshRenderer.sharedMaterials.Length];
                if (isBlackAndWhite)
                {
                    materialsToApply = originalMaterials[i];
                }
                else
                {
                    for (int j = 0; j < materialsToApply.Length; j++)
                    {
                        materialsToApply[j] = blackAndWhiteMaterials[i];
                    }
                }
                meshRenderer.sharedMaterials = materialsToApply;
            }
        }
        //isBlackAndWhite = !isBlackAndWhite; // 更新状态
    }

    private IEnumerator TriggerEndScene()
    {
        yield return new WaitForSeconds(1.5f);
        
        virtualCamera.m_Lens.OrthographicSize = 1.8f;
        yield return new WaitForSeconds(4f);

        // 激活结算 UI 界面
        if (settlementUI != null)
        {
            settlementUI.SetActive(true);
            virtualCamera.m_Lens.OrthographicSize = 5.3f;
        }
        else
        {
            Debug.LogError("Settlement UI GameObject is not assigned.");
        }

    }



}