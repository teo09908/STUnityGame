using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthItem : Item
{
    [Header("Healing Settings")]
    public int healAmount = 10;
    public bool destroyOnUse = true;

    private void Reset()
    {
        // Automatically set up the collider if missing
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    // Called when something enters the trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterStats playerStats = other.GetComponent<CharacterStats>();

        if (playerStats != null && other.CompareTag("Player"))
        {
            ApplyEffect(playerStats);

            if (destroyOnUse)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void ApplyEffect(CharacterStats target)
    {
        if (target == null) return;

        int heal = Mathf.Clamp(healAmount, 0, target.MaxHealth - target.CurrentHealth);
        target.CurrentHealth += heal;

        Debug.Log($"{target.name} healed for {heal} HP using {itemName}");

        // Update health bar if available
        if (target.healthBar != null)
            target.healthBar.SetHealth(target.CurrentHealth);
    }
}
