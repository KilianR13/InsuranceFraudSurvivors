using UnityEngine;

public class Item_Healing : Equippable
{
    [SerializeField] private float healingTimerReduction = 0.3f;
    [SerializeField] private int healingAmmountIncrease = 1;

    private float originalHealingTimer;
    private int originalHealingAmmount;

    private PlayerGameLogic player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform.parent.GetComponentInParent<PlayerGameLogic>();
        originalHealingAmmount = player.healAmmount;
        originalHealingTimer = player.healTimer;
        UpdatePlayerStats();
    }

    void UpdatePlayerStats()
    {
        player.healAmmount = originalHealingAmmount + healingAmmountIncrease;
        if (player.healTimer > 0.5f && !((player.healTimer - healingTimerReduction) < 0.5f))
        {
            player.healTimer = originalHealingTimer - healingTimerReduction;    
        }
        else
        {
            player.healTimer = 0.5f;
        }
    }

    protected override void ApplyUpgrade()
    {
        switch (currentLevel)
        {
            case 2:
                healingAmmountIncrease += 2;    // Healing 3 HP
                break;
            case 3:
                healingAmmountIncrease += 2;    // Healing 5 HP
                break;
            case 4:
                healingTimerReduction += 0.2f;  // 0.5 second reduction
                break;
            case 5:
                healingAmmountIncrease += 3;    // Healing 8 HP
                break;
            case 6:
                healingTimerReduction += 0.3f;  // 0.8 second reduction
                break;
            case 7:
                healingAmmountIncrease += 3;    // Healing 11 HP
                break;
            case 8:
                healingTimerReduction += 0.5f;  // 1.3 second reduction
                break;
            case 9:
                healingAmmountIncrease += 4;    // Healing 15 HP
                healingTimerReduction += 0.7f;  // 2.0 second reduction
                break;
            default:
                Debug.LogError("Wtf?");
                break;
        }
        UpdatePlayerStats();
    }

}
