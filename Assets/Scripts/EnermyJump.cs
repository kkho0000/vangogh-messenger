using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyJump : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    public float bounceHeight = 2f;
    public float bounceFrequency = 5f;
    private float timer = 0f;
    private float timer1 = 0f;
    private const float moveDuration = 3f;
    private bool movingNegativeZ = true;
    private int collisionCount = 0; // 记录碰撞次数
    public GameObject playerObject;


    private void Update()
    {
        timer += Time.deltaTime;
        
        // 水平方向移动
        Vector3 direction = movingNegativeZ ? Vector3.right : Vector3.left;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // 垂直方向弹跳，确保返回值在 0 到 1 之间
        float rawSinValue = Mathf.Abs(Mathf.Sin(Time.time * bounceFrequency));
        float verticalOffset = (rawSinValue+5f) / 2f * bounceHeight;
        Vector3 newPosition = transform.position;
        newPosition.y = verticalOffset;
        transform.position = newPosition;

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
                player.collisionCount = 0;
            }
            
        }
    }

   
}
