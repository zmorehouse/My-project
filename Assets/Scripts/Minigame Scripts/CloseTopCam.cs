using UnityEngine;

public class CloseTopCam : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public Vector3 offset; // Offset to maintain the camera's position above the player

    void Start()
    {
        // Initialize the offset based on the camera's current position
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Update the camera's position to follow the player's X and Z, but keep the Y (height) constant
        Vector3 newPosition = new Vector3(player.position.x + offset.x, transform.position.y, player.position.z + offset.z);
        transform.position = newPosition;
    }
}
