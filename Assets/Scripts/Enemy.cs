using UnityEngine;

public class Enemy : CellObject
{
    private CharacterStats m_Stats;

    [Header("Item Drop Settings")]
    [Range(0f, 1f)] public float dropChance = 0.3f; // 30% chance
    public bool canDropItems = true;

    private void Awake()
    {
        m_Stats = GetComponent<CharacterStats>();
        if (m_Stats == null)
        {
            Debug.LogError("Enemy is missing CharacterStats component!");
        }

        // Subscribe to TurnManager
        GameManager.Instance.TurnManager.OnTick += TurnHappened;

        // Subscribe to death event if CharacterStats has one
        if (m_Stats != null)
            m_Stats.OnDeath += Die;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance?.TurnManager != null)
            GameManager.Instance.TurnManager.OnTick -= TurnHappened;

        if (m_Stats != null)
            m_Stats.OnDeath -= Die;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        // Enemy health etc. are defined in CharacterStats
    }

    public override bool PlayerWantsToEnter()
    {
        var playerStats = GameManager.Instance.PlayerController.GetComponent<CharacterStats>();
        if (playerStats != null && m_Stats != null)
        {
            // Player attacks enemy
            m_Stats.TakeDamage(playerStats.Strength);
        }

        // Player does NOT move into the enemy’s cell
        return false;
    }

    private bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null
            || !targetCell.Passable
            || targetCell.ContainedObject != null)
        {
            return false;
        }

        // Remove from current cell
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        // Move to new cell
        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    private void TurnHappened()
    {
        var playerCell = GameManager.Instance.PlayerController.Cell;

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1)
            || (yDist == 0 && absXDist == 1))
        {
            // Adjacent to player attack
            GetComponent<Animator>().SetTrigger("Attack");

            var playerStats = GameManager.Instance.PlayerController.GetComponent<CharacterStats>();
            if (m_Stats != null && playerStats != null)
            {
                if (m_Stats.TryHit())
                {
                    playerStats.TakeDamage(m_Stats.Strength);
                }
                else
                {
                    Debug.Log("Enemy attack missed!");
                }
            }
        }
        else
        {
            // Move toward player
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
        }
    }

    private bool TryMoveInX(int xDist)
    {
        if (xDist > 0)
            return MoveTo(m_Cell + Vector2Int.right);

        return MoveTo(m_Cell + Vector2Int.left);
    }

    private bool TryMoveInY(int yDist)
    {
        if (yDist > 0)
            return MoveTo(m_Cell + Vector2Int.up);

        return MoveTo(m_Cell + Vector2Int.down);
    }

  
    //  Item Drop System
    
    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (canDropItems && Random.value < dropChance)
        {
            DropRandomItem();
        }

        // Optional: play death animation here

        Destroy(gameObject);
    }

    private void DropRandomItem()
    {
        if (ItemDropManager.Instance == null)
        {
            Debug.LogWarning("No ItemDropManager in scene — cannot drop items.");
            return;
        }

        GameObject itemPrefab = ItemDropManager.Instance.GetRandomDrop();
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Debug.Log($"{gameObject.name} dropped {itemPrefab.name}");
        }
    }
}
