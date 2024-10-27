using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float turnSpeed = 90.0f;
    public float horizontalInput;
    public float forwardInput;
    public bool canMove = false;
    private ShopLogic ShopLogic;
    private DayNightLogic dayNightLogic; // Reference to DayNightLogic
    private float startingY;
    private Rigidbody rb;

    void Start()
    {
        startingY = transform.position.y;
        ShopLogic = FindObjectOfType<ShopLogic>(); // Assuming ShopLogic exists in the scene
        dayNightLogic = FindObjectOfType<DayNightLogic>(); // Assuming DayNightLogic exists in the scene
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || forwardInput != 0)
        {
            float speed = 10.0f;
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput, Space.Self);
            transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput);
            float steeringFactor = Mathf.Clamp01(Mathf.Abs(forwardInput));
            transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput * steeringFactor); 
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        KeepShipWithinBounds();
        MaintainStartingY();
    }

    void KeepShipWithinBounds()
    {
        if (transform.position.x < -50)
        {
            transform.position = new Vector3(-50, startingY, transform.position.z);
        }
        else if (transform.position.x > 50)
        {
            transform.position = new Vector3(50, startingY, transform.position.z);
        }
        else if (transform.position.z < -35)
        {
            transform.position = new Vector3(transform.position.x, startingY, -35);
        }
        else if (transform.position.z > 50)
        {
            transform.position = new Vector3(transform.position.x, startingY, 50);
        }
    }

    void MaintainStartingY()
    {
        transform.position = new Vector3(transform.position.x, startingY, transform.position.z);
    }

    // Interact with DayNightLogic
    public void DecreaseDayLimit()
    {
        if (dayNightLogic != null)
        {
            dayNightLogic.DecreaseDayLimit();
        }
    }

    public void ResetDayLimit()
    {
        if (dayNightLogic != null)
        {
            dayNightLogic.ResetDayLimit();
        }
    }

    public bool CanInteractWithIsland()
    {
        return dayNightLogic != null && dayNightLogic.CanInteractWithIsland();
    }

    public void SetMovement(bool enable)
    {
        canMove = enable;
    }
}
