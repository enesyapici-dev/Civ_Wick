using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float jumpIncreaseForce;
    [SerializeField] private float resetBoostDuration;

    public void Collect()
    {
        playerController.SetJumpForce(jumpIncreaseForce, resetBoostDuration);
        Destroy(gameObject);
    }
}
