using UnityEngine;

// ´æµµµã½Å±¾
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