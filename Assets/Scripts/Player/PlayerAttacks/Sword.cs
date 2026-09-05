using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class of the Sword weapon and it's methods.
/// </summary>
public class Sword : MonoBehaviour
{
    [SerializeField] public int baseDamage = 5;
    [SerializeField] public float damageMultiplier = 0.3f; // Multiplier to multiply the player's speed to increase the damage of the sword.
    private Rigidbody2D playerRb;

    // We save a HashSet of the enemy we just hit so we can prevent hitting the same enemy multiple times. Maybe it's unnecesary.
    private HashSet<EnemyAI> damagedEnemies = new HashSet<EnemyAI>();


    public void SetPlayerRb(Rigidbody2D rb)
    {
        playerRb = rb;
    }


    /// <summary>
    /// Function triggered when it collides with a trigger.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we touch anything that isn't in the correct layer, we ignore it
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy_HitboxHurtbox"))
        {
            return;
        }

        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();
        DamageEnemy(enemy);
    }

    /// <summary>
    /// Function called to calculate and deal damage to a specified enemy.
    /// </summary>
    /// <param name="enemy">Enemy that needs to be damaged</param>
    private void DamageEnemy(EnemyAI enemy)
    {
        if (enemy != null && !damagedEnemies.Contains(enemy))
        {
            float speed = playerRb != null ? playerRb.linearVelocity.magnitude : 0f;
            int finalDamage = baseDamage + Mathf.RoundToInt(speed * damageMultiplier);
            enemy.takeDamage(finalDamage);
            damagedEnemies.Add(enemy);
        }
    }

    /// <summary>
    /// Function used to ensure enemies that died by the sword can be hit once again when re-enabled
    /// </summary>
    /// <param name="enemy">Enemy that needs to be able to be hit again</param>
    public void ClearEnemy(EnemyAI enemy)
    {
        damagedEnemies.Remove(enemy);
    }

    /// <summary>
    /// Function that removes the enemy from the HashSet when the sword leaves it's hitbox.
    /// </summary>
    /// <param name="collision">2D Collider of the enemy</param>
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
