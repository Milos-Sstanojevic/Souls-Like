using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private const float GroundCheckRadius = 0.2f;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;

    private Rigidbody playerRb;
    private PlayerController playerController;
    private CameraController cameraController;
    private Vector2 movementInput = Vector2.zero;
    private Quaternion targetRotation;
    private bool canJump;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    //Callback for new Controller Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    //Callback for new Controller Input System
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!canJump)
            return;

        playerRb.AddForce(Vector3.up * playerController.GetPlayerJumpForce(), ForceMode.Impulse);
        canJump = false;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 moveInput = new Vector3(movementInput.x, 0, movementInput.y).normalized;

        Vector3 moveDirection = GetDirectionOfMovingBasedOnCamera(moveInput);

        ChangeDirectionIfCharacterIsMoving(moveDirection);
    }

    private Vector3 GetDirectionOfMovingBasedOnCamera(Vector3 moveInput) => cameraController.PlanarRotation() * moveInput;

    private void ChangeDirectionIfCharacterIsMoving(Vector3 moveDirection)
    {
        float moveAmount = Mathf.Clamp01(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y));

        if (moveAmount > 0)
        {
            transform.position += Time.deltaTime * movementSpeed * moveDirection;
            targetRotation = Quaternion.LookRotation(moveDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void GroundCheck()
    {
        canJump = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), GroundCheckRadius, groundLayerMask);
    }
}
