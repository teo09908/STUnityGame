using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bossPrefab;

    // Καλείται όταν η πίστα φορτωθεί μέσω EventSystem
    public void SpawnBoss(GameObject levelRoot)
    {
        // Ψάχνουμε το BossSpawnPoint σε όλα τα children του levelRoot
        Transform spawnPoint = FindDeepChild(levelRoot.transform, "BossSpawnPoint");

        if (spawnPoint == null)
        {
            Debug.LogError("Δεν βρέθηκε το BossSpawnPoint!");
            return;
        }

        // Spawn του boss
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);

        // Αν χρειάζεται, ενημερώνουμε την grid position του enemy
        Vector2Int cellPos = WorldToGrid(spawnPoint.position);
        boss.GetComponent<Enemy>().Init(cellPos);
    }

    // Συνάρτηση για να βρει child recursive
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

    // Παράδειγμα μετατροπής world position σε grid position
    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
    }
}
