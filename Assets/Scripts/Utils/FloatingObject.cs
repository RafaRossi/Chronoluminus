using UnityEngine;

[DisallowMultipleComponent]
public class FloatingObject : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatSpeed = 2.0f;

    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private Axis rotationAxis = Axis.Y;

    public float timeOffset = 0f; 

    private Vector3 _startLocalPosition;
    private Quaternion _startLocalRotation;

    private void Start() 
    {
        _startLocalPosition = transform.localPosition;
        _startLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        float t = Time.time + timeOffset;

        float bob = Mathf.Sin(t * floatSpeed) * floatAmplitude;
        transform.localPosition = _startLocalPosition + Vector3.up * bob;

        float angle = rotationSpeed * t;
        Vector3 axisVector = rotationAxis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.up
        };

        transform.localRotation = _startLocalRotation * Quaternion.AngleAxis(angle, axisVector);
    }
}