using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class CinemachineMouseParallax : CinemachineExtension
{
    [SerializeField] private Vector2 intensity;
    [SerializeField] private float smoothness = 5f;

    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private InputActionReference rightStick;

    private Vector2 _currentOffset;
    private Vector2 _targetOffset;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Application.isPlaying)
        {
            if (mousePosition != null) mousePosition.action.Enable();
            if (rightStick != null) rightStick.action.Enable();
        }
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body && Application.isPlaying)
        {
            float normalX = 0f;
            float normalY = 0f;

            Vector2 inputConsole = Vector2.zero;
            if (rightStick != null && rightStick.action != null)
            {
                inputConsole = rightStick.action.ReadValue<Vector2>();
            }

            if (inputConsole.sqrMagnitude > 0.01f)
            {
                normalX = inputConsole.x;
                normalY = inputConsole.y;
            }
            else
            {
                if (mousePosition != null && mousePosition.action != null)
                {
                    Vector2 mousePos = mousePosition.action.ReadValue<Vector2>();

                    normalX = (mousePos.x / Screen.width) * 2f - 1f;
                    normalY = (mousePos.y / Screen.height) * 2f - 1f;

                    normalX = Mathf.Clamp(normalX, -1f, 1f);
                    normalY = Mathf.Clamp(normalY, -1f, 1f);
                }
            }

            _targetOffset = new Vector2(normalX * intensity.x, normalY * intensity.y);
            _currentOffset = Vector2.Lerp(_currentOffset, _targetOffset, deltaTime * smoothness);

            state.PositionCorrection += new Vector3(_currentOffset.x, _currentOffset.y, 0);
        }
    }
}