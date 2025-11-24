using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EXPScript : MonoBehaviour
{
    public int expWorth;
    public AudioSource PickUpSFX;
    public SpriteRenderer sr;
    private bool pickable = true;

    void Start()
    {
        PickUpSFX = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && pickable)
        {
            pickable = false;
            PlayerGameLogic player = collider.GetComponentInParent<PlayerGameLogic>();
            if (player != null)
            {
                player.addEXP(expWorth);
                StartCoroutine(waitForSFX());
            }
        }
    }

    private IEnumerator waitForSFX()
    {   
        PickUpSFX.Play();
        sr.enabled = false;
        yield return new WaitForSecondsRealtime(PickUpSFX.clip.length);
        Destroy(gameObject);
    }
}
