using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject orientationTransform;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private KeyCode movementKey;

    [Header("Jumping Settings")]
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airMultipler;
    [SerializeField] private bool canJump;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airDrag;

    [Header("Slide Settings")]
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private bool isSliding;
    [SerializeField] private float slideMultipler;
    [SerializeField] private float slideDrag;


    [Header("Ground Check Settings")]

    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag;

    //---------------//
    private Rigidbody playerRigidbody;
    private StateController stateController;

    private float horizontalInput, verticalInput;
    private Vector3 movementDirection;

    private void Awake()
    {
        stateController = GetComponent<StateController>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
    }
    private void Update()
    {
        SetInputs();
        SetStates();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey))
        {
            isSliding = true;
        }
        else if (Input.GetKeyDown(movementKey))
        {
            isSliding = false;
        }
        else if (Input.GetKey(jumpKey) && canJump && IsGrounded())
        {
            SetPlayerJumping();
            canJump = false;
            Invoke(nameof(JumpReset), jumpCooldown);
        }


    }

    private void SetStates()
    {
        var movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var isSliding = IsSliding();
        var currentState = stateController.GetCurrentState();

        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
            _ when movementDirection == Vector3.zero && isGrounded && isSliding => PlayerState.SlideIdle,
            _ when movementDirection != Vector3.zero && isGrounded && isSliding => PlayerState.Slide,
            _ when movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
            _ when !isGrounded && !canJump => PlayerState.Jump,
            _ => currentState

        };
        if (newState != currentState)
        {
            stateController.ChangeState(newState);
        }

        Debug.Log(newState);
    }

    private void SetPlayerMovement()
    {
        movementDirection = Vector3.forward * verticalInput + Vector3.right * horizontalInput;

        float forceMultiplier = stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => slideMultipler,
            PlayerState.Jump => airMultipler,
            _ => 1f
        };


        playerRigidbody.AddForce(movementDirection.normalized * movementSpeed * forceMultiplier, ForceMode.Force);


    }

    private void SetPlayerDrag()
    {
        playerRigidbody.linearDamping = stateController.GetCurrentState() switch
        {
            PlayerState.Move => groundDrag,
            PlayerState.Slide => slideDrag,
            PlayerState.Jump => airDrag,
            _ => playerRigidbody.linearDamping
        };

    }

    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.y);

        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            playerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, playerRigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void SetPlayerJumping()
    {
        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void JumpReset()
    {
        canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }
    private bool IsSliding()
    {
        return isSliding;
    }

    public Vector3 GetMovementDirection()
    {
        return movementDirection.normalized;
    }
}
