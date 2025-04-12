using UnityEngine;

// �浵��ű�
public class SavePoint : MonoBehaviour
{

    //[SerializeField] private SubtitleSequenceData sequenceData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the save point.");
            GameManager.Instance.SetLastSavePosition(transform.position);

            //SubtitleManager.Instance.PlaySequence(sequenceData);
        }
    }
}