using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("UI References")]
    public HealthBar healthBar;   // Optional – only used if assigned

    [Header("Stats")]
    public int MaxHealth = 20;
    public int CurrentHealth { get; private set; }

    public int Strength = 3;
    public int Defense = 1;
    public int Speed = 1;
    public int Accuracy = 75; // ποσοστό %
    public int Evasion = 10;  // ποσοστό %
    public int Luck = 0;

    void Awake()
    {
        CurrentHealth = MaxHealth;

        // Only update HealthBar if one exists
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - Defense, 0);

        // Evasion check
        if (Random.Range(0, 100) < Evasion)
        {
            Debug.Log($"{gameObject.name} dodged the attack!");
            return;
        }

        CurrentHealth -= finalDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        // Update bar if available
        if (healthBar != null)
        {
            healthBar.SetHealth(CurrentHealth);
        }

        Debug.Log($"{gameObject.name} took {finalDamage} damage. HP = {CurrentHealth}/{MaxHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public bool TryHit()
    {
        return Random.Range(0, 100) < Accuracy;
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
