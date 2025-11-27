using UnityEngine;

public class SwordUpgrade : MonoBehaviour
{
    [Header("Prefabs y referencias")]
    [SerializeField] private GameObject swordPrefab;       // Prefab de la espada
    [SerializeField] private Transform swordSpawnPoint;    // Empty en el jugador

    private GameObject currentSword;

    // Llamar para intentar spawnear la espada
    public void SwordUpgradeDMG(int upgradeDamage)
    {
        // Si ya existe, aumentamos su daño
        Sword swordComp = currentSword.GetComponent<Sword>();
        if (swordComp != null)
        {
            swordComp.baseDamage += upgradeDamage;
        }
    }

    public void SwordUpgradeMultiplier(float upgradeMultiplier)
    {
        // Si ya existe, aumentamos su daño
        Sword swordComp = currentSword.GetComponent<Sword>();
        if (swordComp != null)
        {
            swordComp.damageMultiplier += upgradeMultiplier;
        }
    }

    public void SpawnSword()
    {
        if (swordPrefab == null || swordSpawnPoint == null) return;

        // Instanciamos la espada
        currentSword = Instantiate(swordPrefab);

        // Buscamos el AttachPoint dentro del prefab
        Transform attachPoint = currentSword.transform.Find("AttachPoint");
        if (attachPoint == null)
        {
            Debug.LogError("No se encontró AttachPoint dentro del prefab de la espada");
            return;
        }

        // Calculamos offset entre el root y el AttachPoint en local space
        Vector3 localOffset = currentSword.transform.InverseTransformPoint(attachPoint.position);

        // Hacemos que la espada sea hija del spawn point del jugador
        currentSword.transform.SetParent(swordSpawnPoint);

        // Posición y rotación relativa al spawn point
        currentSword.transform.localPosition = -localOffset;
        currentSword.transform.localRotation = Quaternion.identity;
        Sword swordComp = currentSword.GetComponent<Sword>();
        if (swordComp != null)
        {
            Rigidbody2D playerRb = swordSpawnPoint.GetComponentInParent<Rigidbody2D>();
            if (playerRb != null)
                swordComp.SetPlayerRb(playerRb);
            else
                Debug.LogWarning("No se encontró Rigidbody2D en el jugador para la espada.");
        }

    }
}
