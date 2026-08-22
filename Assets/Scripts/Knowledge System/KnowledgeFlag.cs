using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Knowledge Flag")]
public class KnowledgeFlag : ScriptableObject
{
    [TextArea] public string description;
}

public class KnowledgeRepository
{
    private readonly HashSet<KnowledgeFlag> _known = new();

    public void Learn(KnowledgeFlag flag) => _known.Add(flag);
    public bool Knows(KnowledgeFlag flag) => _known.Contains(flag);
}