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
    public GameObject targetObject; // Ҫ�л���ȾЧ����Ŀ������
    public Material blackAndWhiteMaterial; // �ڰײ���
    private Material originalMaterial; // ԭʼ����
    public bool isBlackAndWhite = false;
    //public Transform spawnPoint; // �����λ��
    //public string latestSaveFileName;
 
    public bool isDie=false;
    public void Die()
    {
        transform.position = GameManager.Instance.GetLastSavePosition();
        Debug.Log("Player respawned at the last save point.");
    }
    private void Start()
    {
        
        Debug.Log("Player entered the save point.");
        GameManager.Instance.SetLastSavePosition(transform.position);
        
        MeshRenderer renderer = targetObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.sharedMaterial;
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
        MeshRenderer renderer = targetObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            if (isBlackAndWhite)
            {
                renderer.sharedMaterial = originalMaterial;
            }
            else
            {
                renderer.sharedMaterial = blackAndWhiteMaterial;
            }
            //isBlackAndWhite = !isBlackAndWhite;
        }
    }

    /*public void ReturnToSpawnPoint()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }*/
}