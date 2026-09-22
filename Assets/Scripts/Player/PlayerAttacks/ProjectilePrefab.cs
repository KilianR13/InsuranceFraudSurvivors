using UnityEngine;

public class ProjectilePrefab : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifetime = 5f;

    [HideInInspector]
    public int maxEnemyPenetration;
    private int enemiesPenetrated = 0;

    private Vector3 direction;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        lifetime -= Time.deltaTime;

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();

        if (enemy != null)
        {
            enemy.takeDamage(damage);
            enemiesPenetrated++;
            if (enemiesPenetrated == maxEnemyPenetration)
            {
                Destroy(gameObject);    
            }
        }
    }
}