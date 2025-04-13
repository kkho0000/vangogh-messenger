using UnityEngine;

public class SubtitleTrigger : MonoBehaviour
{
    public bool triggerOnce = true;          // �Ƿ������һ��
    private bool hasTriggered = false;
    public SubtitleSequenceData sequenceData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            SubtitleManager.Instance.PlaySequence(sequenceData);
            if (triggerOnce)
            {
                hasTriggered = true; // ����Ϊ�Ѵ���
            }
        }
    }
}

