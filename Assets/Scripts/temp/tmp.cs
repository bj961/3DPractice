using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmp : MonoBehaviour
{
    // CoroutineTest

    private Coroutine myCoroutine;
    private void Start()
    {
        Application.targetFrameRate = 200;
        StartTestCoroutine();
        Invoke("StartTestCoroutine", 1);
    }

    //void StartTestCoroutine()
    //{
    //    if (myCoroutine != null)
    //    {
    //        Debug.Log("코루틴 종료");
    //        StopCoroutine(myCoroutine);
    //    }
    //    myCoroutine = StartCoroutine(TestCoroutine());
    //}

    void StartTestCoroutine()
    {
        StartCoroutine(TestCoroutine());
    }


    IEnumerator TestCoroutine()
    {
        Debug.Log("a");
        yield return null;
        Debug.Log("b");
        yield return new WaitForSeconds(3);
        Debug.Log("c");
    }
}
