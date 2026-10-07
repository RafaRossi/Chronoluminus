using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleDummyData : MonoBehaviour
{
    [SerializeField] private List<BattleUnitAsset> playerUnits = new();
    [SerializeField] private List<BattleUnitAsset> enemyUnits = new();

    private void Start()
    {
        BattleManager.Instance.RegisterUnits(playerUnits, enemyUnits);
    }
}

[Serializable]
public class BattleData
{
    public List<BattleUnitAsset> playerUnits = new();
    public List<BattleUnitAsset> enemyUnits = new();
}
