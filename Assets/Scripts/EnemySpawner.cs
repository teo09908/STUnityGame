using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bossPrefab;

    // �������� ���� � ����� �������� ���� EventSystem
    public void SpawnBoss(GameObject levelRoot)
    {
        // �������� �� BossSpawnPoint �� ��� �� children ��� levelRoot
        Transform spawnPoint = FindDeepChild(levelRoot.transform, "BossSpawnPoint");

        if (spawnPoint == null)
        {
            Debug.LogError("��� ������� �� BossSpawnPoint!");
            return;
        }

        // Spawn ��� boss
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);

        // �� ����������, ������������ ��� grid position ��� enemy
        Vector2Int cellPos = WorldToGrid(spawnPoint.position);
        boss.GetComponent<Enemy>().Init(cellPos);
    }

    // ��������� ��� �� ���� child recursive
    private Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;
            Transform result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    // ���������� ���������� world position �� grid position
    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
    }
}
