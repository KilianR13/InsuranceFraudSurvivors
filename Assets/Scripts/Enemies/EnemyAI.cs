using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float turnSpeed = 200f;
    public float stoppingDistance = 0.5f;
    public float updateRate = 0.2f; // cada cuánto recalcula la dirección al jugador

    public int maxHealth = 1;
    private int currentHealth;

    public GameObject EXPDrop;

    private Vector2 targetDirection;
    private float updateTimer;

    void Update()
    {
        if (player == null) return;

        // Recalcula la dirección solo cada "updateRate" segundos
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            targetDirection = dir;
            updateTimer = updateRate;
        }

        // Gira suavemente hacia la dirección calculada
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Movimiento hacia adelante
        float distance = Vector2.Distance(transform.position, player.position);
        float speed = distance > stoppingDistance ? moveSpeed : 0f;
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            killEnemy();
        }
    }

    // Cuando el pool reactiva el enemigo
    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Instantiate(EXPDrop, transform.position, Quaternion.identity);
            killEnemy();
        }
    }

    public void killEnemy()
    {
        gameObject.SetActive(false); // regresa al pool
    }
}
