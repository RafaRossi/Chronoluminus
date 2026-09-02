using UnityEngine;

[CreateAssetMenu(fileName = "Hour", menuName = "Day System/Hour")]
public class Hour : ScriptableObject
{
    [SerializeField] private string hourText12;
    [SerializeField] private string hourText24;
    
    public string GetHourText12() => hourText12;
    public string GetHourText24() => hourText24;

    public string GetHourText(HourFormat hourFormat = HourFormat._24)
    {
        return hourFormat switch
        {
            HourFormat._12 => hourText12,
            HourFormat._24 => hourText24,
            _ => hourText12
        };
    }
}

public enum HourFormat
{
    _12,
    _24
}