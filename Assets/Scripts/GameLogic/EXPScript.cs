using System.Collections;
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
        if (collider.CompareTag("Player") && pickable) // Checks if what collided with this was the player, and if it can be picked up
        {
            pickable = false; // Set as false to prevent multiple pickups before the sound effect finishes playing
            PlayerGameLogic player = collider.GetComponentInParent<PlayerGameLogic>();
            if (player != null) // Making sure to avoid errors
            {
                player.addEXP(expWorth); // Adds the EXP to the player.
                StartCoroutine(waitForSFX());
            }
        }
    }

    // Plays the pick up sfx, goes invisible, and when the sound finishes, it's deleted.
    private IEnumerator waitForSFX()
    {   
        PickUpSFX.Play();
        sr.enabled = false;
        yield return new WaitForSecondsRealtime(PickUpSFX.clip.length);
        Destroy(gameObject);
    }
}
