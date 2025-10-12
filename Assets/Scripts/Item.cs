using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public abstract void ApplyEffect(CharacterStats target);
}
