using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int baseDamage = 5;
    [SerializeField] private float damageMultiplier = 1f; // cuánto multiplica la velocidad
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
            Debug.Log(playerRb.linearVelocity.magnitude);
            float speed = playerRb != null ? playerRb.linearVelocity.magnitude : 0f;
            int finalDamage = baseDamage + Mathf.RoundToInt(speed * damageMultiplier);
            enemy.takeDamage(finalDamage);
            Debug.Log($"Velocidad del jugador: {speed}");
            Debug.Log($"Daño: {finalDamage}");
            damagedEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy_HitboxHurtbox"))
            return;

        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();
        if (enemy != null && damagedEnemies.Contains(enemy))
        {
            damagedEnemies.Remove(enemy);
        }
    }
}
