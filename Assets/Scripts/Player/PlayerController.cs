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
    public bool canLook = true; //�κ��丮 ���� �� ȭ�� �������� �ʰ� Ŀ�� ������ �ϱ� ����

    public Action inventory;
    private Rigidbody _rigidbody;
    public Animator animator;

    bool isJumping = false;

    private Coroutine coroutine;

    public void JumpingObjectCollisionToggle()
    {
        isJumping = !isJumping;
        //Invoke("JumpingObjectCollisionToggle1", 3f);
        //�ڷ�ƾ���� ���� �΋H���� �� �ٽ� ����ϵ���..?
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
        //if ���� �ƴҶ��� { Move() }
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

        // �� �κ��� �����ؾ� ��.
        // ���� �� �������� �ð��� ����Ͽ� ���� �߿��� move�� ���� ���ϰ� �ϴ°� ���� ������ ���.
        direction = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; // �Է��� ���� �� �� �κ��� 0
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
        if (context.phase == InputActionPhase.Performed) //Started�� Ű ���� �������� �۵�
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
        //���콺 �¿�� �����̸� ���콺��Ÿ�� x�� �����?? ĳ���Ͱ� �¿�� �����̷��� ���� y���� ������ ��
        // �׷��� ������ �޴°�, ���콺��Ÿx�� y�� �־��ְ� y�� x�� �־���� ���ϴ� ����� �� �� ����
        // -> ����嵨Ÿx���� ���콺 �ΰ��� ���ؼ� y�࿡ �־���
        camCurXRot += mouseDelta.y * lookSensivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // Clamp() : �ּҰ����� �۾����� �ִ밪 ��ȯ, �ִ밪���� Ŀ���� �ּҰ� ��ȯ
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // - �ٿ��� ���� : ���콺 �̵��� ȸ���� �ݴ�� �׷� (���� �����̼� �� �ٲ����)

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
