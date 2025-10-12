using UnityEngine;

public class PrefabDatabase : MonoBehaviour
{
    public static PrefabDatabase Instance;

    [Header("Food Prefabs")]
    public GameObject piePrefab; // �� prefab ��� ��� �����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional, �� ������ �� ������ �� ���� �����
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
