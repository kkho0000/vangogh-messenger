using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float timer = 0f;
    private const float moveDuration = 2.2f;
    private bool movingNegativeZ = true;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        Vector3 direction = movingNegativeZ ? Vector3.right : Vector3.left;
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