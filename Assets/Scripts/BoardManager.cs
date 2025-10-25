using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private FoodObject[] m_FoodPrefabs;
    [SerializeField] private Sprite[] m_FoodSprites;
    [SerializeField] private int m_MinFood = 3;
    [SerializeField] private int m_MaxFood = 7;
    private CellData[,] m_BoardData;
    private Tilemap m_Tilemap;
    private Grid m_Grid;
    private List<Vector2Int> m_EmptyCellsList = new List<Vector2Int>();

    [SerializeField] private ExitCellObject ExitCellPrefab;

    [Header("Chest Prefabs")]
    [SerializeField] private GameObject ChestPrefab;   // Strength Chest (Room 2)
    [SerializeField] private GameObject Chest2Prefab;  // Defense Chest (Room 3)

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    [SerializeField] private WallObject[] m_WallPrefabs;

    // -------------------------------------------------------------------
    // INIT
    // -------------------------------------------------------------------
    public void Init(int level)
    {
        Debug.Log($"Init: Width={Width}, Height={Height}");

        if (Width < 3) Width = 3;
        if (Height < 3) Height = 3;

        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        m_EmptyCellsList.Clear();

        m_BoardData = new CellData[Width, Height];

        // Generate floor and walls
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        // Remove player start cell
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        // Exit placement
        Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2);
        if (!m_EmptyCellsList.Contains(endCoord)) m_EmptyCellsList.Add(endCoord);

        if (ExitCellPrefab != null)
        {
            AddObject(Instantiate(ExitCellPrefab), endCoord);
            m_EmptyCellsList.Remove(endCoord);
        }

        GenerateFood(level);
        GenerateWall(level);
        SpawnEnemies(level);
        SpawnChests(level); // spawn chests based on level
    }

    // -------------------------------------------------------------------
    // CHESTS
    // -------------------------------------------------------------------
    private void SpawnChests(int level)
    {
        // Room 2  Strength chest
        if (level == 2 && ChestPrefab != null)
        {
            Vector2Int chest1Pos = GetRandomEmptyCell();
            if (chest1Pos != Vector2Int.zero)
            {
                var chest1 = Instantiate(ChestPrefab);
                AddObject(chest1.GetComponent<CellObject>(), chest1Pos);
                Debug.Log($"Spawned Strength Chest (Room 2) at {chest1Pos}");
            }
        }

        // Room 3  Defense chest
        if (level == 3 && Chest2Prefab != null)
        {
            Vector2Int chest2Pos = GetRandomEmptyCell();
            if (chest2Pos != Vector2Int.zero)
            {
                var chest2 = Instantiate(Chest2Prefab);
                AddObject(chest2.GetComponent<CellObject>(), chest2Pos);
                Debug.Log($"Spawned Defense Chest (Room 3) at {chest2Pos}");
            }
        }
    }

    private Vector2Int GetRandomEmptyCell()
    {
        if (m_EmptyCellsList.Count == 0)
        {
            Debug.LogWarning("No empty cells available!");
            return Vector2Int.zero;
        }

        int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
        Vector2Int coord = m_EmptyCellsList[randomIndex];
        m_EmptyCellsList.RemoveAt(randomIndex);
        return coord;
    }

    // -------------------------------------------------------------------
    // OTHER OBJECTS
    // -------------------------------------------------------------------
    public void SpawnEnemy(Vector2Int coord)
    {
        if (EnemyPrefab == null)
        {
            Debug.LogError("EnemyPrefab not assigned!");
            return;
        }

        GameObject enemyGO = Instantiate(EnemyPrefab);
        AddObject(enemyGO.GetComponent<CellObject>(), coord);
    }

    public void SpawnEnemies(int level)
    {
        int enemyCount = level switch
        {
            1 => 1,
            2 => Random.Range(1, 4),
            3 => Random.Range(2, 5),
            _ => Random.Range(3, 6)
        };

        for (int i = 0; i < enemyCount; i++)
        {
            if (m_EmptyCellsList.Count == 0) break;

            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            m_EmptyCellsList.RemoveAt(randomIndex);

            SpawnEnemy(coord);
        }
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
            return null;

        return m_BoardData[cellIndex.x, cellIndex.y];
    }

    public void GenerateWall(int level)
    {
        int wallCount = Random.Range(6 + level, 10 + level * 2);
        for (int i = 0; i < wallCount; ++i)
        {
            if (m_EmptyCellsList.Count == 0) break;

            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            m_EmptyCellsList.RemoveAt(randomIndex);

            int prefabIndex = Random.Range(0, m_WallPrefabs.Length);
            WallObject newWall = Instantiate(m_WallPrefabs[prefabIndex]);
            AddObject(newWall, coord);
        }
    }

    public void GenerateFood(int level)
    {
        int minFood = m_MinFood + level;
        int maxFood = m_MaxFood + level;
        int foodCount = Random.Range(minFood, maxFood + 1);

        for (int i = 0; i < foodCount; ++i)
        {
            if (m_EmptyCellsList.Count == 0) break;

            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            m_EmptyCellsList.RemoveAt(randomIndex);

            int prefabIndex = Random.Range(0, m_FoodPrefabs.Length);
            FoodObject newFood = Instantiate(m_FoodPrefabs[prefabIndex]);
            AddObject(newFood, coord);
        }
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    void AddObject(CellObject obj, Vector2Int coord)
    {
        if (coord.x < 0 || coord.x >= Width || coord.y < 0 || coord.y >= Height)
        {
            Debug.LogError($"AddObject: Coord {coord} is out of board bounds!");
            Destroy(obj.gameObject);
            return;
        }

        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }

    public void Clean()
    {
        if (m_BoardData == null) return;

        int actualWidth = m_BoardData.GetLength(0);
        int actualHeight = m_BoardData.GetLength(1);

        for (int y = 0; y < actualHeight; ++y)
        {
            for (int x = 0; x < actualWidth; ++x)
            {
                var cellData = m_BoardData[x, y];
                if (cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                    cellData.ContainedObject = null;
                }
                SetCellTile(new Vector2Int(x, y), null);
            }
        }
    }
}
