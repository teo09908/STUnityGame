using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject[] lootItems;

    private bool isOpened = false;

    public void Open(CharacterStats player)
    {
        if (isOpened) return;
        isOpened = true;

        GameObject itemPrefab = lootItems[Random.Range(0, lootItems.Length)];
        Item item = Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity).GetComponent<Item>();
        item.ApplyEffect(player);

        Debug.Log("Chest opened!");
        Destroy(gameObject);
    }
}
