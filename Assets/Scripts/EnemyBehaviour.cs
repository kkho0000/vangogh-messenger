using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float timer = 0f;
    private const float moveDuration = 3f;
    private bool movingNegativeZ = true;
    private int collisionCount = 0; // 记录碰撞次数
    private float timer1 = 0f;
    public GameObject playerObject;

    private void Update()
    {
        timer += Time.deltaTime;

        Vector3 direction = movingNegativeZ ? Vector3.right : Vector3.left;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (timer >= moveDuration)
        {
            movingNegativeZ = !movingNegativeZ;
            timer = 0f;
        }

        PlayerCamera player = playerObject.GetComponent<PlayerCamera>();
        if (!player.isBlackAndWhite)
        {
            timer1 += Time.deltaTime;
            if (timer1 >= 3f)
            {
                player.isBlackAndWhite = true;
                player.ToggleRender();
                timer1 = 0f;
                collisionCount = 0;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCamera player = other.GetComponent<PlayerCamera>();
            if (player != null)
            {
                collisionCount++;
                if (collisionCount == 1)
                {
                    player.isBlackAndWhite = false;
                    player.ToggleRender();
                }
                else if (collisionCount == 2)
                {
                    player.isBlackAndWhite = true;
                    player.ToggleRender();
                    player.Die();
                    //player.ReturnToSpawnPoint();
                    collisionCount = 0; // 重置碰撞次数
                }
            }
        }
    }
}