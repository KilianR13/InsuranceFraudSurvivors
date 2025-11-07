using UnityEngine;

public class EXPScript : MonoBehaviour
{
    public int expWorth;
    public 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerGameLogic player = collider.GetComponentInParent<PlayerGameLogic>();
            if (player != null)
            {
                player.addEXP(expWorth);    
            }
            
            Destroy(gameObject);
        }
    }
}
