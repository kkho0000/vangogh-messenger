using UnityEngine;

// ´æµµµã½Å±¾
public class SavePoint : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the save point.");
            GameManager.Instance.SetLastSavePosition(transform.position);
        }
    }
}