using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float turnSpeed = 200f;
    public float stoppingDistance = 0.5f;
    public float updateRate = 0.2f; // Delay between how often it calculates the player's position

    public int maxHealth = 1;
    private int currentHealth;
    public int damage;

    [Header("Drops & feedback")]
    public GameObject EXPDrop;      // What type of EXP the enemy drops
    public Sprite normalSprite;     // Normal sprite
    public Sprite whiteSprite;      // Sprite for when the player hurts the enemy
    public GameObject deathEffect;
    public bool activated;

    [Header("SFX")]
    public AudioSource hurtSFX;

    private SpriteRenderer sr;


    [HideInInspector] public bool poolable = true;      // Can it return to the pool?
    public int waveIndex { get; set; }                  // Stores what wave this enemy belongs to
    [HideInInspector] public EnemyWaveManager waveManager; 



    private Vector2 targetDirection;
    private float updateTimer;

    void Update()
    {
        if (player == null) return;

        // Recalculate the direction of the enemy every "updateRate" seconds
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            targetDirection = dir;
            updateTimer = updateRate;
        }

        // Steer towards the objective
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Movement forward. No brain, only player
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

    // Pool reactivates the enemy
    void OnEnable()
    {
        activated = true;
        currentHealth = maxHealth;
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();    
        }
        // Making sure the enemy has the correct sprite
        sr.sprite = normalSprite;

        // In case the player has the sword, ensures that the enemy can be hit by it.
        Sword sword = FindFirstObjectByType<Sword>();
        if (sword != null)
        {
            sword.ClearEnemy(this);
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (activated)
        {
            StartCoroutine(FlashWhite());    
            if (currentHealth <= 0)
            {
                StopAllCoroutines();
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
        yield return new WaitForSeconds(0.08f);  // This could be adjusted if the player doesn't notice it enough.
        sr.sprite = normalSprite;
    }

    public void OnRespawn()
    {
        updateTimer = 0f;            // Just to make sure the enemy knows where the player is.
        targetDirection = Vector2.zero;
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }


    public void killEnemy()
    {
        activated = false;
        int currentWaveIndex = waveManager != null ? waveManager.currentWaveIndex : waveIndex;

        // I may remove the death effect. It's really noisy when there are many enemies on screen.
        if (deathEffect != null)
        {
            GameObject explosion = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Animator bulletAnimator = explosion.GetComponent<Animator>();
            float animationLength = 1f;
            if (bulletAnimator != null)
            {
                animationLength = bulletAnimator.GetCurrentAnimatorStateInfo(0).length;
            }
            Destroy(explosion, animationLength);
        }

        GameManager.gm.enemiesDefeated++;
        
        if (!poolable || waveIndex < currentWaveIndex) // If the enemy is from a different wave than the current one.
        {
            Destroy(gameObject); // They are permanently destroyed.
            return;
        }

        if (poolable) // If they are from the current wave, aka can be returned to the pool.
        {
            PoolIdentity p = GetComponent<PoolIdentity>();
            if (p != null && p.prefab != null)
            {
                SimplePool.Return(p.prefab, gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }    
        }
    }


}
