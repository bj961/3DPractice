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
        //direction = Vector3.up;
        direction = transform.up;
        power = 150f;
    }


    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);


        //if (collision.gameObject.CompareTag("Player")) // ���ΰ����ؼ����� ������ٵ� ������.. �� �������� ����
        //{
        //    Debug.Log("Jumpimg Object Collision");
        //    CharacterManager.Instance.Player.controller.JumpingObjectCollisionToggle();
        //    collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);
        //}
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player")) // ���ΰ����ؼ����� ������ٵ� ������.. �� �������� ����
    //    {
    //        Debug.Log("Jumpimg Object CollisionExit");
    //        CharacterManager.Instance.Player.controller.JumpingObjectCollisionToggle();
    //    }
    //}
}
