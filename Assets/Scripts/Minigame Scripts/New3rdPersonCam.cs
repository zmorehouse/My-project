using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public Vector3 offset;   // Offset for the camera's position relative to the player

    public float smoothSpeed = 0.125f; // Adjust this value for smoothing speed
    public float rotationSmoothSpeed = 0.1f; // Adjust this value for rotation smoothing

    void Start()
    {
        // Set an initial offset for the camera
        // offset = new Vector3(0, 2, -5); // Adjust these values based on your needs
    }

    void LateUpdate()
    {
        // Calculate desired position
        Vector3 desiredPosition = player.position + player.TransformDirection(offset);

        // Smoothly interpolate to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Smoothly interpolate the rotation to look at the player
        Quaternion desiredRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed);
    }
}

// using UnityEngine;

// public class Upaded3rdCam : MonoBehaviour
// {
//     public Transform player; // Reference to the player transform
//     public Vector3 offset; // Offset to maintain the camera's position relative to the player
//     public float smoothSpeed = 0.125f; // Smoothing factor for camera movement

//     public float distanceBehind = 5f; // Distance behind the player
//     public float heightAbove = 2f; // Height above the player
    
//     void Start()
//     {
//         // Set the offset based on the initial position of the player
//         // offset = transform.position - player.position;
//     }

//     void LateUpdate()
//     {
        
//         Debug.Log(player.eulerAngles.y);
//         // Check player's Y rotation and flip the camera if needed
//         if (player.eulerAngles.y >= -1 && player.eulerAngles.y <= 1 )
//         {
//             transform.rotation = Quaternion.Euler(0, 0, 0); // Rotate the camera 180 degrees
//             Vector3 desiredPosition = player.position - player.forward * distanceBehind + Vector3.up * heightAbove;

//         // Smoothly transition to the desired position
//             Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//             transform.position = smoothedPosition;
//         }
//         else if (player.eulerAngles.y >= 179 && player.eulerAngles.y <= 181)
//         {
//             transform.rotation = Quaternion.Euler(0, 180, 0); // Reset camera rotation if not facing 0 degrees
//             Vector3 desiredPosition = player.position - player.forward * distanceBehind + Vector3.up * heightAbove;

//         // Smoothly transition to the desired position
//             Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//             transform.position = smoothedPosition;
//         }
//     }
// }