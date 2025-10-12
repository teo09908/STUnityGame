using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    // Event ��� spawn ��� boss
    public static event Action OnBossSpawn;

    // ����� ���� ������ �� ���������� � boss
    public static void BossShouldSpawn()
    {
        OnBossSpawn?.Invoke();
    }
}
