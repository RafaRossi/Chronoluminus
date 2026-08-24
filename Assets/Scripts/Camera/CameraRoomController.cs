using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CylinderEdgeScrollCamera : MonoBehaviour
{
    [Header("Border")]
    [Range(0.01f, 0.4f)]
    [SerializeField] private float horizontalEdgeZone = 0.12f;
    [Range(0.01f, 0.4f)]
    [SerializeField] private float verticalEdgeZone = 0.12f;

    [Header("Yaw")] 
    [SerializeField] private float maxYawSpeed = 40f;
    [SerializeField] private float yawAcceleration = 120f;

    [SerializeField] private bool clampYaw = false;
    [SerializeField] private float maxYawAngle = 90f;

    [Header("Pitch")]
    [SerializeField] private bool enablePitch = false;
    [SerializeField] private float maxPitchSpeed = 25f;
    [SerializeField] private float pitchAcceleration = 100f;
    
    [SerializeField] private float maxPitchAngle = 10f;

    [Header("Gamepad")]
    [Range(0f, 0.5f)]
    [SerializeField] private float gamepadDeadzone = 0.15f;

    private float _currentYawSpeed;
    private float _currentYaw;
    
    private float _currentPitchSpeed;
    private float _currentPitch;

    private void Start()
    {
        _currentYaw = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (CameraInputLock.IsLocked) return;

        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        Vector2 gamepadStick = Gamepad.current != null ? Gamepad.current.rightStick.ReadValue() : Vector2.zero;

        HandleYaw(mousePos, gamepadStick);

        if (enablePitch)
        {
            HandlePitch(mousePos, gamepadStick);
        }
    }

    private float GetMouseYawFactor(Vector2 mousePos)
    {
        float screenW = Screen.width;
        float leftBoundary = screenW * horizontalEdgeZone;
        float rightBoundary = screenW * (1f - horizontalEdgeZone);

        if (mousePos.x <= leftBoundary)
        {
            return -Mathf.InverseLerp(leftBoundary, 0f, mousePos.x);
        }
        if (mousePos.x >= rightBoundary)
        {
            return Mathf.InverseLerp(rightBoundary, screenW, mousePos.x);
        }
        return 0f;
    }

    private float GetGamepadYawFactor(Vector2 stick)
    {
        float x = stick.x;
        if (Mathf.Abs(x) < gamepadDeadzone) return 0f;
        float sign = Mathf.Sign(x);
        float magnitude = Mathf.InverseLerp(gamepadDeadzone, 1f, Mathf.Abs(x));
        return sign * magnitude;
    }

    private void HandleYaw(Vector2 mousePos, Vector2 gamepadStick)
    {
        float mouseFactor = GetMouseYawFactor(mousePos);
        float gamepadFactor = GetGamepadYawFactor(gamepadStick);

        float edgeFactor = Mathf.Abs(gamepadFactor) > Mathf.Abs(mouseFactor) ? gamepadFactor : mouseFactor;

        float targetSpeed = edgeFactor * maxYawSpeed;
        _currentYawSpeed = Mathf.MoveTowards(_currentYawSpeed, targetSpeed, yawAcceleration * Time.deltaTime);

        if (!clampYaw)
        {
            transform.Rotate(Vector3.up, _currentYawSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            _currentYaw = Mathf.Clamp(
                _currentYaw + _currentYawSpeed * Time.deltaTime,
                -maxYawAngle,
                maxYawAngle
            );

            Vector3 euler = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(euler.x, _currentYaw, euler.z);
        }
    }

    private void HandlePitch(Vector2 mousePos, Vector2 gamepadStick)
    {
        float screenH = Screen.height;
        float bottomBoundary = screenH * verticalEdgeZone;
        float topBoundary = screenH * (1f - verticalEdgeZone);

        float mouseFactor = 0f;
        if (mousePos.y <= bottomBoundary)
        {
            mouseFactor = -Mathf.InverseLerp(bottomBoundary, 0f, mousePos.y);
        }
        else if (mousePos.y >= topBoundary)
        {
            mouseFactor = Mathf.InverseLerp(topBoundary, screenH, mousePos.y);
        }

        float gamepadFactor = 0f;
        float y = gamepadStick.y;
        if (Mathf.Abs(y) >= gamepadDeadzone)
        {
            float sign = Mathf.Sign(y);
            float magnitude = Mathf.InverseLerp(gamepadDeadzone, 1f, Mathf.Abs(y));
            gamepadFactor = sign * magnitude;
        }

        float edgeFactor = Mathf.Abs(gamepadFactor) > Mathf.Abs(mouseFactor) ? gamepadFactor : mouseFactor;

        float targetSpeed = edgeFactor * maxPitchSpeed;
        _currentPitchSpeed = Mathf.MoveTowards(_currentPitchSpeed, targetSpeed, pitchAcceleration * Time.deltaTime);

        _currentPitch = Mathf.Clamp(
            _currentPitch + _currentPitchSpeed * Time.deltaTime,
            -maxPitchAngle,
            maxPitchAngle
        );

        Vector3 euler = transform.localEulerAngles;
        float yawForPitch = clampYaw ? _currentYaw : euler.y;
        transform.localRotation = Quaternion.Euler(-_currentPitch, yawForPitch, 0f);
    }
}