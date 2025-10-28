using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WheatDesignSO wheatDesignSO;

    public void Collect()
    {
        playerController.SetMovementSpeed(wheatDesignSO.IncreaceDecreaseMultipler, wheatDesignSO.ResetBoostDuration);
        Destroy(gameObject);
    }
}
