using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager Instance;
    public GameObject[] possibleDrops; // assign prefabs in Inspector

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetRandomDrop()
    {
        if (possibleDrops.Length == 0) return null;
        int i = Random.Range(0, possibleDrops.Length);
        return possibleDrops[i];
    }
}
