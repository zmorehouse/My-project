using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float followRange = 15.0f;
    public float baseFollowSpeed = 4.5f;
    public float speedVariance = 0.75f;
    public float stopDistance = 1.5f;
    public float offsetIntensity = 0.2f;
    public float swayFrequency = 1.5f;
    public float swayAmplitude = 0.5f;
    public float chargeSpeedMultiplier = 2.5f;
    public float chargeShakeIntensity = 0.1f;
    public float chargeShakeDuration = 0.5f;
    public float chargeDistance = 7.5f;  // Distance the enemy will cover during charge

    private GameObject player;
    private SnakeController snakeController;
    private Vector3 initialOffset;
    private float swayOffset;
    private float followSpeed;
    private bool hasCharged = false;
    private bool hasSpottedPlayer = false;
    private bool canChargeAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            snakeController = player.GetComponent<SnakeController>();
        }

        canChargeAttack = Random.Range(0, 4) == 0;

        initialOffset = new Vector3(Random.Range(-offsetIntensity, offsetIntensity), 0, Random.Range(-offsetIntensity, offsetIntensity));
        swayOffset = Random.Range(0f, 2 * Mathf.PI);
        followSpeed = baseFollowSpeed + Random.Range(-speedVariance, speedVariance);
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < followRange && !hasSpottedPlayer)
            {
                hasSpottedPlayer = true;
                if (canChargeAttack)
                {
                    StartCoroutine(ChargeAttack());
                }
                else
                {
                    FollowPlayer();
                }
            }
            else if (hasSpottedPlayer && (!canChargeAttack || hasCharged))
            {
                FollowPlayer();
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position + initialOffset).normalized;
        float sway = Mathf.Sin(Time.time * swayFrequency + swayOffset) * swayAmplitude;
        Vector3 swayDirection = new Vector3(directionToPlayer.z, 0, -directionToPlayer.x) * sway;

        transform.position += (directionToPlayer + swayDirection) * followSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, -0.25f, transform.position.z);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        transform.Rotate(0, -90, 0);
    }

    IEnumerator ChargeAttack()
    {
        hasCharged = true;

        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        transform.Rotate(0, -90, 0);

        float shakeEndTime = Time.time + chargeShakeDuration;
        while (Time.time < shakeEndTime)
        {
            transform.position += Random.insideUnitSphere * chargeShakeIntensity;
            yield return null;
        }

        Vector3 startPosition = transform.position;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float chargeSpeed = followSpeed * chargeSpeedMultiplier;

        while (Vector3.Distance(transform.position, startPosition) < chargeDistance && Vector3.Distance(transform.position, player.transform.position) > stopDistance)
        {
            transform.position += directionToPlayer * chargeSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, -0.25f, transform.position.z);
            yield return null;
        }

        canChargeAttack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            snakeController.TakeDamage(1);
            snakeController.DropResources();
            Debug.Log("Enemy hit the player, taking damage and dropping resources.");
        }
    }
}
