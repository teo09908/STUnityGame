using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthItem : Item
{
    [Header("Healing Settings")]
    public int healAmount = 10;
    public bool destroyOnUse = true;

    private void Start()
    {
        // Auto destroy after 15 seconds if never picked up
        Destroy(gameObject, 15f);
    }


    private void Reset()
    {
        // Automatically set up the collider as trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CharacterStats playerStats = other.GetComponent<CharacterStats>();
        if (playerStats == null) return;

        ApplyEffect(playerStats);

        // Always destroy after pickup (not inside ApplyEffect)
        if (destroyOnUse)
        {
            Debug.Log($"{gameObject.name} consumed and destroyed.");
            Destroy(gameObject);
        }
    }

    public override void ApplyEffect(CharacterStats target)
    {
        if (target == null) return;

        // Heal logic
        int heal = Mathf.Clamp(healAmount, 0, target.MaxHealth - target.CurrentHealth);
        target.CurrentHealth += heal;

        // Update UI
        if (target.healthBar != null)
            target.healthBar.SetHealth(target.CurrentHealth);

        Debug.Log($"{target.name} healed for {heal} HP using {itemName}");
    }
}
