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
        //direction = Vector3.up;
        direction = transform.up;
        power = 150f;
    }


    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);


        //if (collision.gameObject.CompareTag("Player")) // 개인과제해설영상 리지드바디 있으면.. 그 조건으로 변경
        //{
        //    Debug.Log("Jumpimg Object Collision");
        //    CharacterManager.Instance.Player.controller.JumpingObjectCollisionToggle();
        //    collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);
        //}
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player")) // 개인과제해설영상 리지드바디 있으면.. 그 조건으로 변경
    //    {
    //        Debug.Log("Jumpimg Object CollisionExit");
    //        CharacterManager.Instance.Player.controller.JumpingObjectCollisionToggle();
    //    }
    //}
}
