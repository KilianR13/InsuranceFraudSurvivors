using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] public AudioSource enemyDefeatedSFX;

    public IEnumerator playEnemyDefeatedSFX()
    {
        if (enemyDefeatedSFX == null || enemyDefeatedSFX.clip == null)
            yield break;

        enemyDefeatedSFX.Play();
        yield return new WaitForSecondsRealtime(enemyDefeatedSFX.clip.length);
    }
}
