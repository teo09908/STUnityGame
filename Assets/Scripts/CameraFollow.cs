using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // Drag your Player object here in Inspector
    public BoardManager boardManager; // Drag your BoardManager object here in Inspector

    private Vector3 boardCenter;
    private Vector2 safeZoneSize;

    public float smoothSpeed = 0.125f;

    void Start()
    {
        if (boardManager == null)
        {
            Debug.LogError("BoardManager not assigned!");
            return;
        }

        boardCenter = new Vector3(boardManager.Width / 2f, boardManager.Height / 2f, -10f);
        safeZoneSize = new Vector2(3f, 3f); // safe zone around center

        transform.position = boardCenter; // Start camera exactly at board center
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Distance of player from board center
        float deltaX = player.position.x - boardCenter.x;
        float deltaY = player.position.y - boardCenter.y;

        // Start with desired position at board center (so by default camera is locked there)
        Vector3 desiredPosition = boardCenter;

        // ONLY update camera position if player is outside safe zone on X axis
        if (Mathf.Abs(deltaX) > safeZoneSize.x / 2f)
        {
            desiredPosition.x = player.position.x;
        }

        // ONLY update camera position if player is outside safe zone on Y axis
        if (Mathf.Abs(deltaY) > safeZoneSize.y / 2f)
        {
            desiredPosition.y = player.position.y;
        }

        desiredPosition.z = boardCenter.z; // Keep camera Z fixed

        // Smoothly move camera towards desired position (board center or shifted)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
