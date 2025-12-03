using System.Collections;
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
    public int damage;

    [Header("Drops & feedback")]
    public GameObject EXPDrop;
    // public GameObject healthBarPrefab;
    // private HealthBar healthBar;
    public Sprite normalSprite;   // Giulia 1
    public Sprite whiteSprite;    // Giulia 1_white
    public GameObject deathEffect;
    private bool activated;

    [Header("SFX")]
    public AudioSource hurtSFX;

    private SpriteRenderer sr;


    [HideInInspector] public bool poolable = true; // por defecto sí se puede volver al pool
    public int waveIndex { get; set; } // propiedad para guardar la oleada a la que pertenece
    [HideInInspector] public EnemyWaveManager waveManager; // referencia al manager



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
            PlayerGameLogic player = collider.GetComponentInParent<PlayerGameLogic>();
            if (player != null)
            {
                player.takeDamage(damage);    
            }
            killEnemy();
        }
    }

    // Cuando el pool reactiva el enemigo
    void OnEnable()
    {
        activated = true;
        currentHealth = maxHealth;
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();    
        }
        

        // Asegurarte de que vuelve con el sprite normal
        sr.sprite = normalSprite;

    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (activated)
        {
            StartCoroutine(FlashWhite());    
            if (currentHealth <= 0)
            {
                Instantiate(EXPDrop, transform.position, Quaternion.identity);
                killEnemy();
            }
            else
            {
                hurtSFX.Play();
            }
        }
    }

    private IEnumerator FlashWhite()
    {
        sr.sprite = whiteSprite;
        yield return new WaitForSeconds(0.08f);  // Ajustable
        sr.sprite = normalSprite;
    }

    public void killEnemy()
    {
        activated = false;
        int currentWaveIndex = waveManager != null ? waveManager.currentWaveIndex : waveIndex;

        
        if (deathEffect != null)
        {
            GameObject explosion = Instantiate(deathEffect, this.transform.position, Quaternion.identity);
            Animator bulletAnimator = explosion.GetComponent<Animator>();
            float animationLength = 1f; // valor por defecto
            if (bulletAnimator != null)
            {
                animationLength = bulletAnimator.GetCurrentAnimatorStateInfo(0).length;
            }
            Destroy(explosion, animationLength);
        }
        
        if (!poolable || waveIndex < currentWaveIndex) // enemigos de oleadas pasadas
        {
            Destroy(gameObject);
            return;
        }

        if (poolable)
        {
            Poolable p = GetComponent<Poolable>();
            if (p != null && p.originalPrefab != null)
            {
                SimplePool.Return(p.originalPrefab, gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }    
        }
    }


}
