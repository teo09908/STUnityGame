using UnityEngine;

public class Enemy : CellObject
{
    private CharacterStats m_Stats;

    private void Awake()
    {
        m_Stats = GetComponent<CharacterStats>();
        if (m_Stats == null)
        {
            Debug.LogError("Enemy is missing CharacterStats component!");
        }

        GameManager.Instance.TurnManager.OnTick += TurnHappened;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
    }

    public override bool PlayerWantsToEnter()
    {
        var playerStats = GameManager.Instance.PlayerController.GetComponent<CharacterStats>();
        if (playerStats != null && m_Stats != null)
        {
            m_Stats.TakeDamage(playerStats.Strength);
            CheckDeath();
        }

        return false;
    }

    private void TurnHappened()
    {
        if (m_Stats == null || m_Stats.CurrentHealth <= 0)
            return;

        var playerCell = GameManager.Instance.PlayerController.Cell;

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            GetComponent<Animator>().SetTrigger("Attack");

            var playerStats = GameManager.Instance.PlayerController.GetComponent<CharacterStats>();
            if (playerStats != null && m_Stats.TryHit())
            {
                playerStats.TakeDamage(m_Stats.Strength);
                CheckDeath();
            }
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                    TryMoveInY(yDist);
            }
            else
            {
                if (!TryMoveInY(yDist))
                    TryMoveInX(xDist);
            }
        }
    }

    private bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null || !targetCell.Passable || targetCell.ContainedObject != null)
            return false;

        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    private bool TryMoveInX(int xDist)
    {
        return xDist > 0 ? MoveTo(m_Cell + Vector2Int.right) : MoveTo(m_Cell + Vector2Int.left);
    }

    private bool TryMoveInY(int yDist)
    {
        return yDist > 0 ? MoveTo(m_Cell + Vector2Int.up) : MoveTo(m_Cell + Vector2Int.down);
    }

    private void CheckDeath()
    {
        if (m_Stats.CurrentHealth <= 0)
        {
            DropFood();
            Destroy(gameObject);
        }
    }

    private void DropFood()
    {
        var foodPrefab = PrefabDatabase.Instance?.piePrefab;
        if (foodPrefab != null)
        {
            var board = GameManager.Instance.BoardManager;
            var cellPosition = board.CellToWorld(m_Cell);

            GameObject food = Instantiate(foodPrefab, cellPosition, Quaternion.identity);

            var cellData = board.GetCellData(m_Cell);
            if (cellData != null)
            {
                var foodObj = food.GetComponent<CellObject>();
                if (foodObj != null)
                    cellData.ContainedObject = foodObj;
            }
        }
    }
}
