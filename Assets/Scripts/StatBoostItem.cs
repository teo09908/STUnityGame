using UnityEngine;

public class StatBoostItem : Item
{
    public enum StatType { MaxHealth, Strength, Defense, Speed, Accuracy, Evasion, Luck }
    public StatType statType;
    public int boostAmount = 1;

    public override void ApplyEffect(CharacterStats target)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                target.MaxHealth += boostAmount;
                target.Heal(boostAmount); // heal immediately when max health increases
                break;
            case StatType.Strength:
                target.Strength += boostAmount;
                break;
            case StatType.Defense:
                target.Defense += boostAmount;
                break;
            case StatType.Speed:
                target.Speed += boostAmount;
                break;
            case StatType.Accuracy:
                target.Accuracy = Mathf.Min(100, target.Accuracy + boostAmount);
                break;
            case StatType.Evasion:
                target.Evasion = Mathf.Min(100, target.Evasion + boostAmount);
                break;
            case StatType.Luck:
                target.Luck += boostAmount;
                break;
        }

        Debug.Log($"{target.name} gained +{boostAmount} {statType}!");
        Destroy(gameObject);
    }
}
