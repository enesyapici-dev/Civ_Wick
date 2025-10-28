using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour, ICollectible
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WheatDesignSO wheatDesignSO;

    public void Collect()
    {
        playerController.SetJumpForce(wheatDesignSO.IncreaceDecreaseMultipler, wheatDesignSO.ResetBoostDuration);
        Destroy(gameObject);
    }
}
