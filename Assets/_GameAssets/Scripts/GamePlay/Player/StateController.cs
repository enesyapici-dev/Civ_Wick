using System.Data.Common;
using UnityEngine;

public class StateController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerState currentPlayerState = PlayerState.Idle;

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        if (currentPlayerState == newPlayerState) { return; }

        currentPlayerState = newPlayerState;
    }

    public PlayerState GetCurrentState()
    {
        return currentPlayerState;
    }
}
