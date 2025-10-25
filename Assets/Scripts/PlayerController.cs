using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator m_Animator;
    private bool m_IsGameOver;
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private bool m_IsMoving;
    private Vector3 m_MoveTarget;
    public float MoveSpeed = 5.0f;
    public Animator animator;
    public Vector2Int Cell => m_CellPosition;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell, true);
    }

    public void MoveTo(Vector2Int cell, bool immediate)
    {
        m_CellPosition = cell;

        if (immediate)
        {
            m_IsMoving = false;
            transform.position = m_Board.CellToWorld(m_CellPosition);
        }
        else
        {
            m_IsMoving = true;
            m_MoveTarget = m_Board.CellToWorld(m_CellPosition);
        }

        m_Animator.SetBool("Moving", m_IsMoving);
    }

    public void GameOver()
    {
        m_IsGameOver = true;
    }

    public void Damage(int enemyStrength)
    {
        var stats = GetComponent<CharacterStats>();
        stats.TakeDamage(enemyStrength);
        animator.SetTrigger("Damaged");
    }

    public void Init()
    {
        m_IsGameOver = false;
        GameManager.Instance.UpdateStatsUI(); // ενημέρωσε τα στατιστικά στην αρχή
    }

    private void Update()
    {
        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        // Κίνηση προς τον στόχο
        if (m_IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);

            if (transform.position == m_MoveTarget)
            {
                m_IsMoving = false;
                m_Animator.SetBool("Moving", false);

                // Αντικείμενο που υπάρχει στο κελί
                var cellData = m_Board.GetCellData(m_CellPosition);
                if (cellData.ContainedObject != null)
                    cellData.ContainedObject.PlayerEntered();

                // ---- ΝΕΟ: Έλεγχος για αντικείμενα που μπορούν να ληφθούν ----
                CheckForPickup();
            }
            return;
        }

        // --- WAIT TURN ---
        if ((Keyboard.current.spaceKey != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
            (Keyboard.current.numpad5Key != null && Keyboard.current.numpad5Key.wasPressedThisFrame))
        {
            GameManager.Instance.TurnManager.Tick();
            return;
        }

        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            TryMoveTo(newCellTarget);
        }
    }

    private void TryMoveTo(Vector2Int newCellTarget)
    {
        BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

        if (cellData != null && cellData.Passable)
        {
            GameManager.Instance.TurnManager.Tick();

            if (cellData.ContainedObject == null)
            {
                MoveTo(newCellTarget, false);
            }
            else if (cellData.ContainedObject.PlayerWantsToEnter())
            {
                MoveTo(newCellTarget, false);
                cellData.ContainedObject.PlayerEntered();
            }
        }
    }

    private void CheckForPickup()
    {
        // Έλεγξε αν υπάρχει αντικείμενο HealthItem στην τρέχουσα θέση
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (var hit in hits)
        {
            HealthItem item = hit.GetComponent<HealthItem>();
            if (item != null)
            {
                CharacterStats stats = GetComponent<CharacterStats>();
                if (stats != null)
                {
                    item.ApplyEffect(stats);
                    return; // Μόλις πάρεις ένα αντικείμενο, βγες
                }
            }
        }
    }

    public void Attack()
    {
        m_Animator.SetTrigger("Attack");
    }
}
