using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform

    // Boundary limits
    private float minX = -18f;
    private float maxX = 18f;
    private float minZ = 4f;
    private float maxZ = 40f;

    void Update()
    {
        // Clamp the player's position within the defined boundaries
        float clampedX = Mathf.Clamp(playerTransform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(playerTransform.position.z, minZ, maxZ);

        // Apply the clamped position
        playerTransform.position = new Vector3(clampedX, playerTransform.position.y, clampedZ);
    }
}
