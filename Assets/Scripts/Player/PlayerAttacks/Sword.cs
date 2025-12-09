using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] public int baseDamage = 5;
    [SerializeField] public float damageMultiplier = 0.3f; // cuánto multiplica la velocidad
    private Rigidbody2D playerRb;

    // Guardamos los enemigos que ya recibieron daño mientras estén dentro
    private HashSet<EnemyAI> damagedEnemies = new HashSet<EnemyAI>();


    public void SetPlayerRb(Rigidbody2D rb)
    {
        playerRb = rb;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Filtramos por layer
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy_HitboxHurtbox"))
        {
            return;
        }

        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();
        if (enemy != null && !damagedEnemies.Contains(enemy))
        {
            float speed = playerRb != null ? playerRb.linearVelocity.magnitude : 0f;
            int finalDamage = baseDamage + Mathf.RoundToInt(speed * damageMultiplier);
            enemy.takeDamage(finalDamage);
            damagedEnemies.Add(enemy);
        }
    }
    public void ClearEnemy(EnemyAI enemy)
    {
        damagedEnemies.Remove(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy_HitboxHurtbox"))
        {
            return;
        }

        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();
        if (enemy != null && damagedEnemies.Contains(enemy))
        {
            damagedEnemies.Remove(enemy);
        }
    }
}
