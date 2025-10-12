using UnityEngine;

public class PrefabDatabase : MonoBehaviour
{
    public static PrefabDatabase Instance;

    [Header("Food Prefabs")]
    public GameObject piePrefab; // Το prefab που ήδη έχεις

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional, αν θέλεις να μείνει σε κάθε σκηνή
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
