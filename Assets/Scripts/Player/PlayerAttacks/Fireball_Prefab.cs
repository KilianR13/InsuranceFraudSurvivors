using UnityEngine;

public class FireBall_Prefab : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 10;

    private Vector3 direction;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        lifetime -= Time.deltaTime;

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("FIREBALL TOCÓ: " + collision.gameObject.name);
        Debug.Log("LAYER: " + LayerMask.LayerToName(collision.gameObject.layer));
        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();

        if (enemy != null)
        {
            Debug.Log("ENCONTRÓ ENEMIGO: " + enemy.gameObject.name);
            enemy.takeDamage(damage);
            Destroy(gameObject);
        }
    }
}