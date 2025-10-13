using UnityEngine;

public class FoodObject : CellObject
{
    [Header("Food Settings")]
    public int AmountGranted = 10;  // πόσο food δίνει

    public override void PlayerEntered()
    {
        Destroy(gameObject);

        // Αύξηση food
        GameManager.Instance.ChangeFood(AmountGranted);

        
    }
}
