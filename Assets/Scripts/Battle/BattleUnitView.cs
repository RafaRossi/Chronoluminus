using UnityEngine;

public class BattleUnitView : MonoBehaviour
{
    [SerializeField] private Transform targetCameraPoint;

    public Transform GetTargetCameraPoint()
    {
        return targetCameraPoint;
    }
}
