using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    public Tile EndTile;

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        GameManager.Instance.BoardManager.SetCellTile(coord, EndTile);
    }

    public override bool PlayerWantsToEnter()
    {
        // Allow player to enter this cell (the exit)
        return true;
    }

    public override void PlayerEntered()
    {
        // Called when player enters the exit cell, start new level
        GameManager.Instance.NewLevel();
    }
}
