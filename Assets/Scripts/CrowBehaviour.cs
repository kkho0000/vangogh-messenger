using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowBehaviour : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveDuration = 2.2f;
    public Vector3 initialDirection = Vector3.right; // 初始飞行方向
    private float timer = 0f;
    private bool movingNegativeZ = true;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        Vector3 direction = movingNegativeZ ? initialDirection : -initialDirection;
        // transform.Translate(direction * moveSpeed * Time.deltaTime);
        rb.velocity = direction * moveSpeed;

        if (timer >= moveDuration)
        {
            movingNegativeZ = !movingNegativeZ;
            timer = 0f;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}