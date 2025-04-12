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
    
    public GameObject[] targetObjects; // Ҫ�л���ȾЧ����Ŀ����������
    public Material[] blackAndWhiteMaterials; // �ڰײ�������
    private Material[][] originalMaterials; // ԭʼ�������飬��ά�����Լ��ݲ�ͬ��Ⱦ��
    public bool isBlackAndWhite = false;
    public bool isDie = false;
    public Image blackScreen;
    public CinemachineVirtualCamera virtualCamera;
    Animator animator;
    public GameObject confiner;
    public GameObject settlementUI; // ���� UI ����� GameObject
    public void Die()
    {
        transform.position = GameManager.Instance.GetLastSavePosition();
        Debug.Log("Player respawned at the last save point.");
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
        //isBlackAndWhite = !isBlackAndWhite; // ����״̬
    }

    private IEnumerator TriggerEndScene()
    {
        blackScreen.gameObject.SetActive(true); // ��ʾ����
        yield return new WaitForSeconds(0.8f); // �������� 1 ��
        GameObject finishObject = GameObject.FindWithTag("Finish");
        virtualCamera.m_Lens.OrthographicSize = 1.8f;
        confiner.SetActive(false);
        blackScreen.gameObject.SetActive(false); // �رպ���
        
        
        yield return new WaitForSeconds(5f);

        // ������� UI ����
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End")) // �����յ��ǩΪ "End"
        {
            StartCoroutine(TriggerEndScene());
            Destroy(other.gameObject);
        }
    }
}