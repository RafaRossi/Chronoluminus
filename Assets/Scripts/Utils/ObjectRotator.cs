using System.Collections;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private bool useLocalRotation = true;
    [SerializeField] private Vector3 standardTargetRotation = new Vector3(0f, 0f, 0f);

    private Quaternion _originalRotation = Quaternion.identity;
    private Coroutine _currentRoutine;

    private void Awake()
    {
        _originalRotation = useLocalRotation ? transform.localRotation : transform.rotation;
    }

    [ContextMenu("Rotate Object")]
    public void SetRotation()
    {
        SetRotation(standardTargetRotation, false, 0f);
    }

    public void SetRotation(Vector3 eulerAngles, bool additive = false, float duration = 0f)
    {
        SetRotation(Quaternion.Euler(eulerAngles), additive, duration);
    }

    public void SetRotation(Quaternion targetRotation, bool additive, float duration = 0f)
    {
        Quaternion finalTarget = targetRotation;
        
        if (additive)
        {
            Quaternion current = useLocalRotation ? transform.localRotation : transform.rotation;
            finalTarget = current * targetRotation;
        }

        if (duration <= 0f)
        {
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            ApplyRotationDirectly(finalTarget);
        }
        else
        {
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            _currentRoutine = StartCoroutine(SmoothRotateRoutine(finalTarget, duration));
        }
    }

    private IEnumerator SmoothRotateRoutine(Quaternion targetRotation, float duration)
    {
        Quaternion startRot = useLocalRotation ? transform.localRotation : transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            Quaternion smoothedRot = Quaternion.Slerp(startRot, targetRotation, t);
            ApplyRotationDirectly(smoothedRot);
            
            yield return null;
        }

        ApplyRotationDirectly(targetRotation);
        _currentRoutine = null;
    }

    private void ApplyRotationDirectly(Quaternion rot)
    {
        if (useLocalRotation)
        {
            transform.localRotation = rot;
        }
        else
        {
            transform.rotation = rot;
        }
    }
    
    public void ResetRotation(float duration = 0f)
    {
        SetRotation(_originalRotation, false, duration);
    }
}