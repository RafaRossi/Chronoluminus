using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DiscreteHourController : HourController<DiscreteHourController>
{
    [SerializeField] private List<Hour> orderedHours;
    
    private readonly Dictionary<Hour, UnityAction> _hourEvents = new();

    private Queue<Hour> _hours = new();

    [SerializeField] private UnityEvent<Hour> onHourStarted = new();
    
    public Hour CurrentHour { get; private set; }

    public void StartDay()
    {
        _hours = new Queue<Hour>();
        _hourEvents.Clear();
        
        foreach (var orderedHour in orderedHours)
        {
            _hours.Enqueue(orderedHour);
        }
        
        OnDayStarted?.Invoke();
        
        AdvanceHour();
    }
    
    public void RegisterHourEvent(Hour hour, UnityAction action)
    {
        if (_hourEvents.ContainsKey(hour))
        {
            _hourEvents[hour] += action;
        }
        else
        {
            _hourEvents.TryAdd(hour, action);
        }
    }

    public void UnregisterHourEvent(Hour hour, UnityAction action)
    {
        if (_hourEvents.ContainsKey(hour))
        {
            _hourEvents[hour] -= action;
        }
    }

    [ContextMenu("Advance Hour")]
    public void AdvanceHour()
    {
        if(_hours.Count <= 0) return;
        
        CurrentHour = _hours.Dequeue();

        if (_hourEvents.TryGetValue(CurrentHour, out var hourEvent))
        {
            hourEvent?.Invoke();
        }
        
        hourText.text = CurrentHour.GetHourText(hourFormat);
        //hourAnimator.Play("Show Hour");
        
        onHourStarted?.Invoke(CurrentHour);
    }
}