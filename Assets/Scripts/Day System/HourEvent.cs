using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HourEvent : MonoBehaviour
{
    [SerializeField] private Dictionary<Hour, UnityEvent> hourEvents = new();
    
    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        foreach (var hourEventsPair in hourEvents)
        {
            DiscreteHourController.Instance.RegisterHourEvent(hourEventsPair.Key, hourEventsPair.Value.Invoke);
        }
    }
    
    private void UnregisterEvents()
    {
        foreach (var hourEventsPair in hourEvents)
        {
            DiscreteHourController.Instance.UnregisterHourEvent(hourEventsPair.Key, hourEventsPair.Value.Invoke);
        }
    }
}
