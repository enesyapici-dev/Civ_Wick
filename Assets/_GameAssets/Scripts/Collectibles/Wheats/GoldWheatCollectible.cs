using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float movementIncreaseSpeed;
    [SerializeField] private float resetBoostDuration;

    public void Collect()
    {
        playerController.SetMovementSpeed(movementIncreaseSpeed, resetBoostDuration);
        Destroy(gameObject);
    }
}
