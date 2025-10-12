using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("UI References")]
    public HealthBar healthBar;   // Σύνδεσε το HealthBar από το UI

    [Header("Stats")]
    public int MaxHealth = 20;
    public int CurrentHealth { get; private set; }

    public int Strength = 3;
    public int Defense = 1;
    public int Speed = 1;
    public int Accuracy = 75; // %
    public int Evasion = 10;  // %
    public int Luck = 0;

    private void Awake()
    {
        CurrentHealth = MaxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - Defense, 0);

        if (Random.Range(0, 100) < Evasion)
        {
            Debug.Log($"{gameObject.name} dodged the attack!");
            return;
        }

        CurrentHealth -= finalDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

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

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(CurrentHealth);
        }

        Debug.Log($"{gameObject.name} healed {amount} HP. HP = {CurrentHealth}/{MaxHealth}");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
