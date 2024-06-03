using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float defaultMoveSpeed;
    public float moveSpeed;
    public float jumpPower = 80f;
    public float dashSpeed = 10f;
    private Vector2 curMovementInput;
    [SerializeField] private Vector3 beforeDirection;
    public LayerMask groundLayerMask;
    public float jumpStamina;
    public float dashStamina = 10f;
    

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensivity;
    private Vector2 mouseDelta;
    public bool canLook = true; //인벤토리 켰을 때 화면 움직이지 않고 커서 나오게 하기 위함

    public Action inventory;
    private Rigidbody _rigidbody;
    public Animator animator;

    bool isJumping = false;

    private Coroutine coroutine;

    public void JumpingObjectCollisionToggle()
    {
        isJumping = !isJumping;
        //Invoke("JumpingObjectCollisionToggle1", 3f);
        //코루틴으로 땅에 부딫혔을 때 다시 토글하도록..?
        GroundedToggle();
    }


    public void GroundedToggle()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(JumpingObjectCollisionToggle1());
    }

    private IEnumerator JumpingObjectCollisionToggle1()
    {
        while (!IsGrounded())
        {
            isJumping = !isJumping;
            yield return null;
        }
    }



    private void Awake()
    {
        cameraContainer = transform.Find("CameraContainer").GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = defaultMoveSpeed;
    }


    void FixedUpdate()
    {
        //if 점프 아닐때만 { Move() }
        if(isJumping == false)
        {
            Move();
        }
        
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }



    private void Move()
    {
        Vector3 direction;

        // 이 부분을 수정해야 함.
        // 점프 시 떨어지는 시간을 계산하여 점프 중에는 move를 동작 안하게 하는게 가장 간단한 방법.
        direction = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; // 입력이 없을 때 이 부분이 0
        direction *= moveSpeed;
        direction.y = _rigidbody.velocity.y;

        //_rigidbody.velocity = direction;

        if (direction != Vector3.zero)
        {
            _rigidbody.velocity = direction;
            beforeDirection = direction;
        }
        else
        {
            if (direction != beforeDirection)
            {
                _rigidbody.velocity = direction;
                beforeDirection = direction;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) //Started는 키 눌린 순간에만 작동
        {
            curMovementInput = context.ReadValue<Vector2>();
            animator.SetBool("Moving", true);
            //animator.SetFloat("xAxis", curMovementInput.x);
            animator.SetFloat("zAxis", curMovementInput.y);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            animator.SetBool("Moving", false);
            //animator.SetFloat("xAxis", curMovementInput.x);
            animator.SetFloat("zAxis", curMovementInput.y);
        }
    }

    void CameraLook()
    {
        //마우스 좌우로 움직이면 마우스델타의 x가 변경됨?? 캐릭터가 좌우로 움직이려면 축을 y축을 돌려야 함
        // 그래서 실제로 받는값, 마우스델타x는 y에 넣어주고 y는 x에 넣어줘야 원하는 결과를 낼 수 있음
        // -> 마우드델타x값에 마우스 민감도 곱해서 y축에 넣어줌
        camCurXRot += mouseDelta.y * lookSensivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // Clamp() : 최소값보다 작아지면 최대값 반환, 최대값보다 커지면 최소값 반환
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // - 붙여준 이유 : 마우스 이동과 회전이 반대라서 그럼 (실제 로테이션 값 바꿔봐라)

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensivity, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && CharacterManager.Instance.Player.condition.UseStamina(dashStamina))
        {
            Debug.Log("Dash");
            animator.SetBool("isRunning", true);
            moveSpeed += dashSpeed;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("isRunning", false);
            moveSpeed = defaultMoveSpeed;
        }
    }


    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up*0.01f), Vector3.down),
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up*0.01f), Vector3.down),
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
