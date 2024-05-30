using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Vector3 direction;
    public float time;
    public float directionChangeTime;
    public float speed;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        direction = Vector3.forward;
        speed = 5f;
        time = 0;
        directionChangeTime = 1.5f;
    }

    void Movement(Transform targetTransform)
    {
        targetTransform.position = targetTransform.position + direction * speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (time >= directionChangeTime)
        {
            time = 0;
            direction = -direction;
        }
        time += Time.deltaTime;
        Movement(transform);
    }

    private void OnCollisionStay(Collision collision)
    {
        Movement(collision.gameObject.transform);
    }
}
