using UnityEngine;

public class FoodObject : CellObject
{
    [Header("Food Settings")]
    public int AmountGranted = 10;  // πόσο food δίνει
    public int HealthGranted = 5;   // πόση ζωή δίνει

    public override void PlayerEntered()
    {
        Destroy(gameObject);

        // Αύξηση food
        GameManager.Instance.ChangeFood(AmountGranted);

        // Αύξηση ζωής
        var playerStats = GameManager.Instance.PlayerController.GetComponent<CharacterStats>();
        if (playerStats != null)
        {
            playerStats.Heal(HealthGranted);
        }
    }
}
