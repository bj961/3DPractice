using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public enum CameraMode
{
    FirstPersonView,
    ThirdPersonView
}

public class PlayerCameraController : MonoBehaviour
{
    public Camera _camera;
    public CameraMode cameraMode;

    Vector3 cameraPosition;
    Vector3 FPSView = new Vector3(0, -1.75f, -0.135f);
    Vector3 TPSView = new Vector3(0, -2f, 10f);

    [SerializeField] private float cameraTurnSpeed = 15f;
    private float mouseX;
    private float mouseY;


    void Start()
    {
        cameraMode = CameraMode.ThirdPersonView;
        cameraPosition = TPSView;
    }


    void LateUpdate()
    {
        CameraFollow();
    }


    void CameraFollow()
    {
        Vector3 Distance = cameraPosition;
        _camera.transform.position = transform.position - _camera.gameObject.transform.rotation * cameraPosition;// Distance;
        
    }

    void CameraChange()
    {
        switch (cameraMode)
        {
            case CameraMode.FirstPersonView:
                cameraPosition = FPSView;
                break;
            case CameraMode.ThirdPersonView:
                cameraPosition = TPSView;
                break;
        }
    }

    public void OnCameraChange(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            switch (cameraMode)
            {
                case CameraMode.FirstPersonView:
                    cameraMode = CameraMode.ThirdPersonView;
                    break;
                case CameraMode.ThirdPersonView:
                    cameraMode = CameraMode.FirstPersonView;
                    break;
            }
            CameraChange();
        }
        
    }
}
