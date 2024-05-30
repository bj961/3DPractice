using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingObject : MonoBehaviour
{
    public Vector3 direction;
    public float power;


    void Start()
    {
        // TODO : ������Ʈ�� �� �������� �� ���ϵ��� �����Ұ�
        // ��) �밢������ ������ ������Ʈ�� �밢������ �����ϵ���
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
