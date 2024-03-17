using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private const string GroundTag = "Ground";
    private PlayerMovementController playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovementController>();
    }
}
