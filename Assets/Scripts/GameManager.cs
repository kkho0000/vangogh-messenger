using UnityEngine;

// 游戏管理脚本
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private Vector3 lastSavePosition;

    public void SetLastSavePosition(Vector3 position)
    {
        lastSavePosition = position;
    }

    public Vector3 GetLastSavePosition()
    {
        return lastSavePosition;
    }
}