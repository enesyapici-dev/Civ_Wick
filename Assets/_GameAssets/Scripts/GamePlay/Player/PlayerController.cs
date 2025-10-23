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
    [SerializeField] private bool canJump;
    [SerializeField] private float jumpCooldown;

    [Header("Slide Settings")]
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private float slideMultipler;
    [SerializeField] private bool isSliding;
    [SerializeField] private float slideDrag;


    [Header("Ground Check Settings")]

    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag;

    //---------------//
    private Rigidbody playerRigidbody;

    private float horizontalInput, verticalInput;
    private Vector3 movementDirection;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
    }
    private void Update()
    {
        setInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void setInputs()
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
        else if (Input.GetKey(jumpKey) && canJump && isGrounded())
        {
            SetPlayerJumping();
            canJump = false;
            Invoke(nameof(JumpReset), jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        movementDirection = Vector3.forward * verticalInput + Vector3.right * horizontalInput;

        if (isSliding)
        {
            playerRigidbody.AddForce(movementDirection.normalized * movementSpeed * slideMultipler, ForceMode.Force);
        }
        else
        {
            playerRigidbody.AddForce(movementDirection.normalized * movementSpeed, ForceMode.Force);
        }
    }

    private void SetPlayerDrag()
    {
        if (isSliding)
        {
            playerRigidbody.linearDamping = slideDrag;
        }
        else
        {

            playerRigidbody.linearDamping = groundDrag;
        }
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

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }
}
