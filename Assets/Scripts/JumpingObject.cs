using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingObject : MonoBehaviour
{
    public Vector3 direction;
    public float power;


    void Start()
    {
        // TODO : 오브젝트의 위 방향으로 힘 가하도록 수정할것
        // 예) 대각선으로 기울어진 오브젝트면 대각선으로 점프하도록
        direction = Vector3.up;
        power = 150f;
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Jumpimg Object Collision");
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);
        }
    }
}
