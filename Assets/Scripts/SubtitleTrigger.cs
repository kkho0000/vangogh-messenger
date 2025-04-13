using UnityEngine;

public class SubtitleTrigger : MonoBehaviour
{
    public bool triggerOnce = true;          // 是否仅触发一次
    private bool hasTriggered = false;
    public SubtitleSequenceData sequenceData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            SubtitleManager.Instance.PlaySequence(sequenceData);
            if (triggerOnce)
            {
                hasTriggered = true; // 设置为已触发
            }
        }
    }
}

