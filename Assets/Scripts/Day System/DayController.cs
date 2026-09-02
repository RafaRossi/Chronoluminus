using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DayController : MonoBehaviour
{
    [SerializeField] private List<Hour> orderedHours;
    
    [SerializeField] private HourFormat hourFormat;
    [SerializeField] private TMP_Text hourText;
    
    [SerializeField] private Animator hourAnimator;
    
    private readonly Dictionary<Hour, UnityAction> _hourEvents = new();

    private Queue<Hour> _hours = new();

    public static readonly UnityAction OnDayStarted = delegate { };
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

    [ContextMenu("Advance Hour")]
    public void AdvanceHour()
    {
        CurrentHour = _hours.Dequeue();

        if (_hourEvents.TryGetValue(CurrentHour, out var hourEvent))
        {
            hourEvent?.Invoke();
        }
        
        hourText.text = CurrentHour.GetHourText(hourFormat);
        hourAnimator.Play("Show Hour");
        
        onHourStarted?.Invoke(CurrentHour);
    }
}