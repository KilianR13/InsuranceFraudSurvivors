using UnityEngine;

public class PlayerGameLogic : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private int currentEXP;
    private int currentLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = 0;
        currentEXP = 0;
        currentHealth = maxHealth;
        
    }

    public void addEXP(int exp)
    {
        currentEXP += exp;
        Debug.Log($"Current EXP: {currentEXP}");
    }

    public void takeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            GameManager.gm.loseGame();
            return;
        }
        currentHealth -= damage;
        Debug.Log($"Current health: {currentHealth}");
    }
}
