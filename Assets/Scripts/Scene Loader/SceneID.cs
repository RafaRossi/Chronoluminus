using UnityEngine;

[CreateAssetMenu(menuName = "Scenes/Scene ID" )]
public class SceneID : ScriptableObject
{
    [field:SerializeField] public string SceneName { get; private set; }
}
