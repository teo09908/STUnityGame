using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    public bool isOpened = false;
    public string chestName = "Chest";

    public enum RewardType { None, Strength, Defense, HealthItem, CustomItem }

    [Header("Reward Settings")]
    public RewardType rewardType = RewardType.None;
    public int rewardAmount = 5;

    [Header("Optional Item Prefab")]
    public GameObject itemPrefab; // e.g. sword, shield, etc.

    [Header("Animation")]
    public Animator animator;

    private float loopTimer = 0f;
    private bool isLoopOpen = false;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        if (animator != null)
            animator.Play("ChestGlow", 0, 0f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened) return;

        if (other.CompareTag("Player"))
        {
            CharacterStats playerStats = other.GetComponent<CharacterStats>();
            OpenChest(playerStats);
        }
    }

    private void OpenChest(CharacterStats playerStats)
    {
        if (isOpened || playerStats == null) return;
        isOpened = true;

        Debug.Log($"{chestName} opened! Reward: {rewardType} +{rewardAmount}");

        // Give reward
        switch (rewardType)
        {
            case RewardType.Strength:
                playerStats.Strength += rewardAmount;
                break;
            case RewardType.Defense:
                playerStats.Defense += rewardAmount;
                break;
            case RewardType.HealthItem:
                playerStats.Heal(rewardAmount);
                break;
            case RewardType.CustomItem:
                if (itemPrefab != null)
                    Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
                break;
        }

        // Stop looping and play final open animation
        if (animator != null)
            animator.Play("ChestOpenClose", 0, 0f);

        GameManager.Instance.UpdateStatsUI();

        // vanish after short delay
        Destroy(gameObject, 0.6f);
    }
}
