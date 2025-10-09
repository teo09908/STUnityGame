using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- Game Systems ---
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public UIDocument UIDoc;
    public TurnManager TurnManager { get; private set; }

    // --- Game Stats ---
    private int m_FoodAmount = 50;
    private int m_CurrentLevel = 0;
    private int m_Health = 10;
    private int m_MaxHealth = 10;
    private int m_Strength = 3;
    private int m_Defense = 1;
    private int m_Speed = 5;

    // --- UI Elements ---
    private VisualElement m_HUDPanel;
    private VisualElement m_GameOverPanel;

    private Label m_FoodLabel;
    private Label m_HealthLabel;
    private Label m_StrengthLabel;
    private Label m_DefenseLabel;
    private Label m_SpeedLabel;

    private Label m_GameOverMessage;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        var root = UIDoc.rootVisualElement;

        // --- Panels ---
        m_HUDPanel = root.Q<VisualElement>("HUDPanel");
        m_GameOverPanel = root.Q<VisualElement>("GameOverPanel");

        // --- HUD Labels ---
        m_FoodLabel = root.Q<Label>("FoodLabel");
        m_HealthLabel = root.Q<Label>("HealthLabel");
        m_StrengthLabel = root.Q<Label>("StrengthLabel");
        m_DefenseLabel = root.Q<Label>("DefenseLabel");
        m_SpeedLabel = root.Q<Label>("SpeedLabel");

        // --- Game Over UI ---
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");

        // Εξασφάλιση ότι το HUD φαίνεται, και το GameOver είναι κρυφό στην αρχή
        m_HUDPanel.style.visibility = Visibility.Visible;
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        StartNewGame();
    }

    // === MAIN GAME LOGIC ===

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        m_CurrentLevel = 0;
        m_FoodAmount = 50;
        m_Health = 10;
        m_MaxHealth = 10;
        m_Strength = 3;
        m_Defense = 1;
        m_Speed = 5;

        UpdateHUD();

        BoardManager.Clean();
        BoardManager.Init(m_CurrentLevel); //  Πρέπει να γίνει πριν το Spawn

        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1)); //  περνάμε το BoardManager

        NewLevel();
    }


    public void NewLevel()
    {
        m_CurrentLevel++;
        BoardManager.Width = 10 + m_CurrentLevel;
        BoardManager.Height = 10 + m_CurrentLevel;

        Debug.Log($"New Level: {m_CurrentLevel} (Board: {BoardManager.Width}x{BoardManager.Height})");

        BoardManager.Clean();
        BoardManager.Init(m_CurrentLevel);

        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
        UpdateHUD();
    }

    // === TURN MANAGEMENT ===

    private void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    // === STATS MODIFIERS ===

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        if (m_FoodAmount <= 0)
        {
            GameOver();
            return;
        }
        UpdateHUD();
    }

    public void ChangeHealth(int amount)
    {
        m_Health = Mathf.Clamp(m_Health + amount, 0, m_MaxHealth);
        if (m_Health <= 0)
        {
            GameOver();
            return;
        }
        UpdateHUD();
    }

    public void ChangeStrength(int amount)
    {
        m_Strength = Mathf.Max(0, m_Strength + amount);
        UpdateHUD();
    }

    public void ChangeDefense(int amount)
    {
        m_Defense = Mathf.Max(0, m_Defense + amount);
        UpdateHUD();
    }

    public void ChangeSpeed(int amount)
    {
        m_Speed = Mathf.Max(1, m_Speed + amount);
        UpdateHUD();
    }

    // === HUD UPDATE ===

    private void UpdateHUD()
    {
        m_FoodLabel.text = $"Food: {m_FoodAmount}";
        m_HealthLabel.text = $"Health: {m_Health}/{m_MaxHealth}";
        m_StrengthLabel.text = $"Strength: {m_Strength}";
        m_DefenseLabel.text = $"Defense: {m_Defense}";
        m_SpeedLabel.text = $"Speed: {m_Speed}";
    }

    // === GAME OVER ===

    private void GameOver()
    {
        PlayerController.GameOver();
        m_GameOverPanel.style.visibility = Visibility.Visible;
        m_GameOverMessage.text = $"Game Over!\n\nSurvived {m_CurrentLevel} levels";
    }

    // === GETTERS (αν τα χρειαστείς αλλού) ===
    public int Food => m_FoodAmount;
    public int Health => m_Health;
    public int Strength => m_Strength;
    public int Defense => m_Defense;
    public int Speed => m_Speed;

    public void UpdateStatsUI()
    {
        // Παίρνουμε το component CharacterStats από τον Player
        var stats = PlayerController.GetComponent<CharacterStats>();
        if (stats == null)
        {
            Debug.LogWarning("CharacterStats component not found on PlayerController!");
            return;
        }

        // Ενημέρωση των Labels στο UI
        if (m_HealthLabel != null)
            m_HealthLabel.text = $"Health: {stats.CurrentHealth}/{stats.MaxHealth}";
        if (m_StrengthLabel != null)
            m_StrengthLabel.text = $"Strength: {stats.Strength}";
        if (m_DefenseLabel != null)
            m_DefenseLabel.text = $"Defense: {stats.Defense}";
        if (m_SpeedLabel != null)
            m_SpeedLabel.text = $"Speed: {stats.Speed}";
    }

}
