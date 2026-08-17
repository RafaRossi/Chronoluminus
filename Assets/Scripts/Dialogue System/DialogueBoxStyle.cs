using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Dialogue/Box Style")]
    public class DialogueBoxStyle : ScriptableObject
    {
        public TMP_FontAsset Font;
        public Color TextColor = Color.white;
        public Sprite BoxBackground;
        public float CharactersPerSecond = 40f;
    }
}