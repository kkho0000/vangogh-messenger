using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static SavePoint;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[ExecuteInEditMode]
public class PlayerCamera : MonoBehaviour
{
    public GameObject[] targetObjects; // 要切换渲染效果的目标物体数组
    public Material[] blackAndWhiteMaterials; // 黑白材质数组
    private Material[][] originalMaterials; // 原始材质数组，二维数组以兼容不同渲染器
    public bool isBlackAndWhite = false;
    public bool isDie = false;

    public void Die()
    {
        transform.position = GameManager.Instance.GetLastSavePosition();
        Debug.Log("Player respawned at the last save point.");
    }

    private void Start()
    {
        Debug.Log("Player entered the save point.");
        GameManager.Instance.SetLastSavePosition(transform.position);

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
}