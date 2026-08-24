using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerSideScrollController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    [Header("Movement")]
    [SerializeField] private CharacterController controller;
    
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float acceleration = 25f;

    [Header("Input")]
    [SerializeField] private InputActionReference moveActionReference;

    [Header("Z Axis")]
    [SerializeField] private bool lockZPosition = true;
    [SerializeField] private float fixedZ = 0f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    [Header("Visual")]
    [SerializeField] private Transform spriteRoot;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private float _currentSpeedX;
    private float _verticalVelocity;
    private int _facingSign = 1;

    private void Awake()
    {
        if (spriteRoot == null) spriteRoot = transform;
    }

    private void OnEnable()
    {
        moveActionReference.action.Enable();
    }

    private void OnDisable()
    {
        moveActionReference.action.Disable();
    }

    private void Update()
    {
        if (CameraInputLock.IsLocked)
        {
            ApplyGravityOnly();
            return;
        }

        float moveInput = GetMoveInputX();

        float targetSpeed = moveInput * moveSpeed;
        _currentSpeedX = Mathf.MoveTowards(_currentSpeedX, targetSpeed, acceleration * Time.deltaTime);

        UpdateFacing(moveInput);
        UpdateAnimator();

        Move(_currentSpeedX);
    }

    private float GetMoveInputX()
    {
        return moveActionReference.action.ReadValue<Vector2>().x;
    }

    private void UpdateFacing(float moveInput)
    {
        if (Mathf.Abs(moveInput) < 0.01f) return;

        int newSign = moveInput > 0f ? 1 : -1;
        if (newSign != _facingSign)
        {
            _facingSign = newSign;
            Vector3 scale = spriteRoot.localScale;
            scale.x = Mathf.Abs(scale.x) * _facingSign;
            spriteRoot.localScale = scale;
        }
    }

    private void UpdateAnimator()
    {
        if (!animator) return;
        
        float normalizedSpeed = moveSpeed > 0f ? Mathf.Abs(_currentSpeedX) / moveSpeed : 0f;
        animator.SetFloat(Speed, normalizedSpeed);
    }

    private void Move(float speedX)
    {
        Vector3 motion = Vector3.right * speedX;

        if (controller.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }
        _verticalVelocity += gravity * Time.deltaTime;
        motion.y = _verticalVelocity;

        controller.Move(motion * Time.deltaTime);

        if (lockZPosition)
        {
            Vector3 pos = transform.position;
            pos.z = fixedZ;
            transform.position = pos;
        }
    }

    private void ApplyGravityOnly()
    {
        if (controller.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }
        _verticalVelocity += gravity * Time.deltaTime;
        controller.Move(new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

        _currentSpeedX = 0f;
        
        if (animator) animator.SetFloat(Speed, 0f);
    }
}