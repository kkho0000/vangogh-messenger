using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float timer = 0f;
    private const float moveDuration = 3f;
    private bool movingNegativeZ = true;
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
            if (timer1 >= 1f)
            {
                player.isBlackAndWhite = true;
                player.ToggleRender();
                timer1 = 0f;
                player.collisionCount = 0;
            }

        }
        
    }

}