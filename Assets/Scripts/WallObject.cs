// Opws kai to FoodObject -> inherits apto CellObject
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public int MaxHealth = 3;

    private int m_HealthPoint;
    private Tile m_OriginalTile;
    public Tile DamagedTiles;  // holds damaged versions of the wall

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_HealthPoint = MaxHealth;

        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        // Call the Attack method on the PlayerController
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
            // Update tile to damaged version when 1 HP left
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, DamagedTiles);
            return false;  // Wall still blocking
        }

        if (m_HealthPoint <= 0)
        {
            // Restore original tile and destroy wall object
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
            Destroy(gameObject);
            return true;  // Player can enter now
        }

        return false;
    }

}
