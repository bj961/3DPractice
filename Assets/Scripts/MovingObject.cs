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

        //direction = Vector3.forward;
        speed = 5f;
        time = 0;
        directionChangeTime = 3f;
    }

    void FixedUpdate()
    {
        if (time >= directionChangeTime)
        {
            time = 0;
            direction = -direction;
        }
        time += Time.deltaTime;
        _rigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
        //transform.position = transform.position + direction * speed * Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        //collision.gameObject.GetComponent<Rigidbody>().velocity += transform.GetComponent<Rigidbody>().velocity;
    }
}
