using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AutoAttackFireball : MonoBehaviour
{
    [Header("Ataque")]
    public float attackRange = 5f;        // Rango de detección de enemigos
    public float attackCooldown = 1.5f;   // Tiempo entre ataques
    public GameObject fireballPrefab;      // Prefab de la bola de fuego
    public Transform firePoint;         // Punto desde donde se inicia el ataque.

    private float cooldownTimer = 0f;
    private PlayerGameLogic player;   // <--- referencia al jugador

    private void Start()
    {
        player = GetComponent<PlayerGameLogic>(); // obtiene el daño bonus
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            EnemyAI nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                ShootFireball(nearestEnemy);
                cooldownTimer = attackCooldown;
            }
        }
    }

    EnemyAI FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        EnemyAI nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 pos = transform.position;

        foreach (GameObject enemyObj in enemies)
        {
            if (!enemyObj.activeInHierarchy) continue;
            float dist = Vector3.Distance(pos, enemyObj.transform.position);
            if (dist < minDist && dist <= attackRange)
            {
                minDist = dist;
                nearest = enemyObj.GetComponent<EnemyAI>();
            }
        }

        return nearest;
    }

    void ShootFireball(EnemyAI enemy)
    {
        if (fireballPrefab == null || firePoint == null) return;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        FireBall proj = fireball.GetComponent<FireBall>();
        if (proj != null)
        {
            int baseDamage = fireballPrefab.GetComponent<FireBall>().damage;
            proj.damage = baseDamage + player.FireBallBonusDMG;

            proj.SetTarget(enemy.transform);
        }
    }
}
