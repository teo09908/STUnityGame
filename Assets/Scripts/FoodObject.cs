using UnityEngine;

public class FoodObject : CellObject
{
    [Header("Food Settings")]
    public int AmountGranted = 10;  // ���� food �����
    public int HealthGranted = 5;   // ���� ��� �����

    public override void PlayerEntered()
    {
        Destroy(gameObject);

        // ������ food
        GameManager.Instance.ChangeFood(AmountGranted);

        // ������ ����
        var playerStats = GameManager.Instance.PlayerController.GetComponent<CharacterStats>();
        if (playerStats != null)
        {
            playerStats.Heal(HealthGranted);
        }
    }
}
