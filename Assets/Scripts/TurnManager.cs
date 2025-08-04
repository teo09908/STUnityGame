public class TurnManager
{
    public event System.Action OnTick;
    private int m_Turn;

    public void Tick()
    {
        m_Turn++;
        UnityEngine.Debug.Log("Turn: " + m_Turn);
        OnTick?.Invoke();
    }

    public int GetTurn()
    {
        return m_Turn;
    }
}
