using System;
using FMODUnity;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue System/Speaker Data")]
public class SpeakerData : ScriptableObject
{
    public string key;
    public Sprite sprite;

    public EventReference typingSound;
    public TMP_FontAsset customFont;
}