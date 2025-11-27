using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;

    private Transform target;
    public int damage;   // Se asigna al crear la fireball
    private bool hasTarget = false;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasTarget = true;
    }

    void Update()
    {
        if (!hasTarget || target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Movimiento hacia el objetivo
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotación hacia el enemigo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Impacto
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            HitTarget();
        }

        // Tiempo de vida
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }

    void HitTarget()
    {
        EnemyAI enemy = target.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
