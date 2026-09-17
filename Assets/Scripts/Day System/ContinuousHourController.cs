using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class HourController<T> : Singleton<T> where T : HourController<T>
{
    [SerializeField] protected HourFormat hourFormat;
    [SerializeField] protected TMP_Text hourText;
    
    public static readonly UnityAction OnDayStarted = delegate { };
}

[Serializable]
public class ScheduledEvent
{
    public string label;
    [Range(0f, 1439f)] public float triggerMinute;
    public UnityEvent onTrigger;
    [NonSerialized] public bool hasTriggeredToday;
}

public class ContinuousHourController : HourController<ContinuousHourController>
{
    [Header("Time Settings")]
    [SerializeField] private float startMinute = 420f;          
    [SerializeField] private float dayLengthInRealSeconds = 1700f;
    
    [Header("Scheduled Events")]
    [SerializeField] private List<ScheduledEvent> scheduledEvents = new();
    [SerializeField] private UnityEvent<int> onHourChanged = new();
    
    public float CurrentMinute { get; private set; }
    public bool IsRunning { get; private set; }
 
    private int _lastWholeHour = -1;
    private float _minutesPerSecond;
 
    protected override void Awake()
    {
        base.Awake();
        _minutesPerSecond = 1440f / dayLengthInRealSeconds;
    }
    
    public void StartDay()
    {
        CurrentMinute = startMinute;
        _lastWholeHour = -1;
 
        foreach (var scheduledEvent in scheduledEvents)
            scheduledEvent.hasTriggeredToday = false;
 
        IsRunning = true;
        OnDayStarted?.Invoke();
        UpdateDisplay();
    }
    
    private void Update()
    {
        if (!IsRunning) return;
 
        var previousMinute = CurrentMinute;
        CurrentMinute += Time.deltaTime * _minutesPerSecond;
 
        if (CurrentMinute >= 1440f)
        {
            CurrentMinute = 1440f;
            IsRunning = false;
        }
 
        CheckScheduledEvents(previousMinute, CurrentMinute);
        CheckHourChanged();
        UpdateDisplay();
    }
    
    private void CheckScheduledEvents(float previous, float current)
    {
        foreach (var scheduledEvent in scheduledEvents)
        {
            if (scheduledEvent.hasTriggeredToday) continue;
 
            var crossed = current >= previous
                ? scheduledEvent.triggerMinute >= previous && scheduledEvent.triggerMinute < current
                : scheduledEvent.triggerMinute >= previous || scheduledEvent.triggerMinute < current;

            if (!crossed) continue;
            
            scheduledEvent.hasTriggeredToday = true;
            scheduledEvent.onTrigger?.Invoke();
        }
    }
    
    private void CheckHourChanged()
    {
        int wholeHour = Mathf.FloorToInt(CurrentMinute / 60f) % 24;
        if (wholeHour != _lastWholeHour)
        {
            _lastWholeHour = wholeHour;
            onHourChanged?.Invoke(wholeHour);
        }
    }
    
    private void UpdateDisplay()
    {
        hourText.text = FormatTime(CurrentMinute, hourFormat);
    }
    
    private static string FormatTime(float totalMinutes, HourFormat format)
    {
        int hour = Mathf.FloorToInt(totalMinutes / 60f) % 24;
        int minute = Mathf.FloorToInt(totalMinutes % 60f);
 
        if (format == HourFormat._12)
        {
            int hour12 = hour % 12;
            if (hour12 == 0) hour12 = 12;
            string suffix = hour < 12 ? "AM" : "PM";
            return $"{hour12:00}:{minute:00} {suffix}";
        }
 
        return $"{hour:00}:{minute:00}";
    }
}
