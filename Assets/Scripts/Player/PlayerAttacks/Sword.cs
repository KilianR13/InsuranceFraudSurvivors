using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    // Guardamos los enemigos que ya recibieron daño mientras estén dentro
    private HashSet<EnemyAI> damagedEnemies = new HashSet<EnemyAI>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Filtramos por layer
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy_HitboxHurtbox"))
        {
            return;
        }

        Debug.Log(collision);
        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();
        if (enemy != null && !damagedEnemies.Contains(enemy))
        {
            enemy.takeDamage(damage);
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
