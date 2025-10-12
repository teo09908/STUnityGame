using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    // Event για spawn του boss
    public static event Action OnBossSpawn;

    // Κλήση όταν πρέπει να εμφανιστεί ο boss
    public static void BossShouldSpawn()
    {
        OnBossSpawn?.Invoke();
    }
}
