using UnityEngine;

public class SwordUpgrade : MonoBehaviour
{
    [Header("Prefabs y referencias")]
    [SerializeField] private GameObject swordPrefab;       // Prefab de la espada
    [SerializeField] private Transform swordSpawnPoint;    // Empty en el jugador

    private GameObject currentSword;

    // Llamar para intentar spawnear la espada
    public void TrySpawnSword(int currentXP)
    {
        if (currentSword != null) return; // ya tenemos la espada

        if (currentXP >= 0) // requisito para obtener la espada
        {
            SpawnSword();
        }
    }

    private void SpawnSword()
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

    }
}
