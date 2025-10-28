using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "WheatDesignSo", menuName = "ScriptableObjects/WheatDesignSO")]
public class WheatDesignSO : ScriptableObject
{
    [SerializeField] private float increaceDecreaseMultipler;
    [SerializeField] private float resetBoostDuration;

    public float IncreaceDecreaseMultipler => increaceDecreaseMultipler;
    public float ResetBoostDuration => resetBoostDuration;
}
