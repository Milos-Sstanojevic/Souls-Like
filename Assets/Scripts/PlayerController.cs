using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public float GetPlayerSpeed() => speed;
    public float GetPlayerJumpForce() => jumpForce;
}
