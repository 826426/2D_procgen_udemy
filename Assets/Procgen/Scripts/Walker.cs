using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Walker : MonoBehaviour
{
    [Header("Tilemap References")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap backgroundTilemap;

    [Header("Tile References")]
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase backgroundTile;

    [Header("Generation Parameters")]
    [SerializeField] private int maxIterations = 100;
    [SerializeField] private int walkersCount = 1;
    [SerializeField][Range(0f, 1f)] private float directionChangeChance = 1.0f;
    [SerializeField] private Vector2Int startPositionOffset = new Vector2Int(0, 0);
    [SerializeField] private Vector2Int customGridSize = new Vector2Int(20, 20);
    [SerializeField] private int borderSize = 3;
    [SerializeField] private PlayerHandler playerHander;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private ObjectPlacer enemyPlacer;
    [SerializeField] private ExitHandler exitHandler;


    private bool[,] visitedTiles;
    private Vector2Int gridSize;
    private Vector2Int gridMin;
    private Vector2Int gridMax;
    private Vector2Int walkerMin;
    private Vector2Int walkerMax;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        CalculateGridBounds();
        GenerteLevel();
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CalculateGridBounds();
            GenerteLevel();

            if(playerHander != null)
            {
                playerHander.RegeneratePlayer();
            }
            if(exitHandler != null)
            {
                exitHandler.RegenerateExit();
            }
            if(objectPlacer != null)
            {
                objectPlacer.OnLevelGenerated();
            }
            if(enemyPlacer != null)
            {
                enemyPlacer.OnLevelGenerated();
            }
        }
    }

    void Update()
    {
        
    }

    private void GenerteLevel()
    {
        groundTilemap.ClearAllTiles();
        backgroundTilemap.ClearAllTiles();

        visitedTiles = new bool[gridSize.x, gridSize.y];

        Vector2Int startPos = new Vector2Int(
            walkerMin.x + (walkerMax.x - walkerMin.x) / 2 + startPositionOffset.x,
            walkerMin.y + (walkerMax.y - walkerMin.y) / 2 + startPositionOffset.y
        );

        startPos.x = Mathf.Clamp(startPos.x, walkerMin.x, walkerMax.x);
        startPos.y = Mathf.Clamp(startPos.y, walkerMin.y, walkerMax.y);

        for(int i = 0; i < walkersCount; i++)
        {
            RandomWalker(startPos);
        }

        FillTilpmaps();

        if (objectPlacer != null)
        {
            objectPlacer.OnLevelGenerated();
        }
        if(enemyPlacer != null)
        {
            enemyPlacer.OnLevelGenerated();
        }
    }

    private void FillTilpmaps()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x + gridMin.x, y + gridMin.y, 0);
                if (visitedTiles[x, y])
                {
                    groundTilemap.SetTile(tilePos, floorTile);
                }
                else
                {
                    backgroundTilemap.SetTile(tilePos, backgroundTile);
                }
            }
        }
    }

    private void CalculateGridBounds()
    {
        gridSize = new Vector2Int(
            Mathf.Max(2 * borderSize + 1, customGridSize.x), 
            Mathf.Max(2 * borderSize + 1, customGridSize.y)
        );

        gridMin = new Vector2Int(-gridSize.x / 2, -gridSize.y / 2);
        gridMax = new Vector2Int(gridMin.x + gridSize.x - 1, gridMin.y + gridSize.y - 1);

        walkerMin = new Vector2Int(gridMin.x + borderSize, gridMin.y + borderSize);
        walkerMax = new Vector2Int(gridMax.x - borderSize, gridMax.y - borderSize);

        walkerMin.x = Mathf.Min(walkerMin.x, walkerMax.x);
        walkerMin.y = Mathf.Min(walkerMin.y, walkerMax.y);
        walkerMax.x = Mathf.Max(walkerMin.x, walkerMax.x);
        walkerMax.y = Mathf.Max(walkerMin.y, walkerMax.y);
    }

    private void RandomWalker(Vector2Int startPos)
    {
        Vector2Int currentPos = startPos;
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
        };

        Vector2Int currentDirection = directions[Random.Range(0, directions.Length)];

        if (IsWithinWalkerBounds(currentPos))
        {
            int arrayX = currentPos.x - gridMin.x;
            int arrayY = currentPos.y - gridMin.y;
            visitedTiles[arrayX, arrayY] = true;
        }

        for(int i = 0; i < maxIterations; i++)
        {
            if(Random.value < directionChangeChance)
            {
                currentDirection = directions[Random.Range(0, directions.Length)];
            }

            currentPos += currentDirection;

            currentPos.x = Mathf.Clamp(currentPos.x, walkerMin.x, walkerMax.x);
            currentPos.y = Mathf.Clamp(currentPos.y, walkerMin.y, walkerMax.y);

            if (IsWithinWalkerBounds(currentPos))
            {
                int arrayX = currentPos.x - gridMin.x;
                int arrayY = currentPos.y - gridMin.y;
                if(!visitedTiles[arrayX, arrayY])
                {
                    visitedTiles[arrayX, arrayY] = true;
                }
            }
        }
    }

    private bool IsWithinWalkerBounds(Vector2Int pos)
    {
        return pos.x >= walkerMin.x && pos.x <= walkerMax.x && pos.y >= walkerMin.y && pos.y <= walkerMax.y;
    }

    public Vector2Int GetGridMin() => gridMin;
    public Vector2Int GetGridMax() => gridMax;

    public void TriggerRestart()
    {
        CalculateGridBounds();
        GenerteLevel();

        if (playerHander != null)
        {
            playerHander.RegeneratePlayer();
        }
        if (exitHandler != null)
        {
            exitHandler.RegenerateExit();
        }
        if (objectPlacer != null)
        {
            objectPlacer.OnLevelGenerated();
        }
        if (enemyPlacer != null)
        {
            enemyPlacer.OnLevelGenerated();
        }
    }
}
