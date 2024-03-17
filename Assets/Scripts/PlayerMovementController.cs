using System.Collections.Specialized;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private const float FallMultiplier = 2.0f;
    [SerializeField] private Transform playerCamera;
    private Rigidbody playerRb;
    private PlayerController playerController;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped;
    private bool canJump;
    private float distanceToGround = 3f;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Awake()
    {
        Physics.gravity *= 2;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
        Jump();
    }

    private void Jump()
    {
        if (canJump && jumped)
        {
            playerRb.AddForce(Vector3.up * playerController.GetPlayerJumpForce(), ForceMode.Impulse);
            canJump = false;
        }
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            transform.Translate(playerController.GetPlayerSpeed() * Time.deltaTime * moveDir.normalized, Space.World);
        }
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f))
            canJump = true;

        else
            canJump = false;
    }
}
