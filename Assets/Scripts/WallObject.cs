using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public int MaxHealth = 3;

    private int m_HealthPoint;
    private Tile m_OriginalTile;
    public Tile DamagedTiles;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HealthPoint = MaxHealth;

        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Attack();
            }
        }

        m_HealthPoint -= 1;

        if (m_HealthPoint == 1)
        {
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, DamagedTiles);
            return false;
        }

        if (m_HealthPoint <= 0)
        {
            // 30% drop chance
            float dropChance = 0.3f;
            if (Random.value < dropChance && ItemDropManager.Instance != null)
            {
                GameObject itemPrefab = ItemDropManager.Instance.GetRandomDrop();
                if (itemPrefab != null)
                {
                    GameObject drop = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                    drop.transform.SetParent(GameManager.Instance.BoardManager.transform); // make it child of board

                }
            }

            // Restore tile and remove wall
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
