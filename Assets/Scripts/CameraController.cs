using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static Vector3 DistanceForCamera = new Vector3(0, 0, 5);
    private const float MinVerticalAngle = -2;
    private const float MaxVerticalAngle = 45;
    [SerializeField] private Vector2 framingOffset;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float mouseSpeedVertical;
    [SerializeField] private float mouseSpeedHorizontal;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;
    private float rotationY;
    private float rotationX;
    private float invertXValue;
    private float invertYValue;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        invertXValue = invertX ? -1 : 1;
        invertYValue = invertY ? -1 : 1;

        rotationY += Input.GetAxis("Mouse X") * mouseSpeedHorizontal * invertXValue;
        rotationX += Input.GetAxis("Mouse Y") * mouseSpeedVertical * invertYValue;
        rotationX = Mathf.Clamp(rotationX, MinVerticalAngle, MaxVerticalAngle);

        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        Vector3 focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.SetPositionAndRotation(focusPosition - targetRotation * DistanceForCamera, targetRotation);
    }

    public Quaternion PlanarRotation() => Quaternion.Euler(0, rotationY, 0);
}
