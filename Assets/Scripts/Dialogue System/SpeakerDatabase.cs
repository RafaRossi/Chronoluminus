using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue System/Speaker Database")]
public class SpeakerDatabase : ScriptableObject
{
    [SerializeField] private List<SpeakerData> speakers = new();

    public Dictionary<string, SpeakerData> GetSpeakers()
    {
        return speakers.ToDictionary(speakerData => speakerData.key);
    }
}
