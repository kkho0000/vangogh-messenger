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
    private Material[] originalMaterials; // 原始材质数组
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

        originalMaterials = new Material[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            SpriteRenderer renderer = targetObjects[i].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                originalMaterials[i] = renderer.sharedMaterial;
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
            SpriteRenderer renderer = targetObjects[i].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                if (isBlackAndWhite)
                {
                    renderer.sharedMaterial = originalMaterials[i];
                }
                else
                {
                    renderer.sharedMaterial = blackAndWhiteMaterials[i];
                }
            }
        }
        //isBlackAndWhite = !isBlackAndWhite; // 更新状态
    }
}