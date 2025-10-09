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
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        // � ��� ��� ������ ����� �������� ��� �� CharacterStats
    }

    // ���� � ������� ��������� �� ���� ��� ���� �� �����
    public override bool PlayerWantsToEnter()
    {
        var playerStats = GameManager.Instance.PlayerController.GetComponent<CharacterStats>();
        if (playerStats != null && m_Stats != null)
        {
            // � ������� ���������� ���� �����
            m_Stats.TakeDamage(playerStats.Strength);
        }

        // � ������� ��� ������� ��� ���� ���� �� ��� �����
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

        // �������� ��� ������ ��� �� ������ ����
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        // �������� ��� ��� ����
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
            // ����� ���� ������ -> �������
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
            // ������ ���� ��� ������
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
}
