using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Generates a random maze using a depth-first search (DFS) algorithm and places the player and goal in the maze.
/// </summary>
public class RandomMazeGenerator : MonoBehaviour
{
    [Header("Maze Configuration")]
    public Tile[] houseWallTiles;
    public Tile[] floorTiles;

    [Header("Player and Goal")]
    public GameObject player;
    public GameObject goalPrefab;

    [Header("Maze Size")]
    [SerializeField]
    private int width = 20;
    [SerializeField]
    private int height = 17;

    private Tilemap[] houseTilemaps;
    private Tilemap floorTilemap;
    private Vector3Int playerStartPosition;
    private Vector3Int goalPosition;

    private int[,] maze;
    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    void Start()
    {
        SetupTilemaps();
        GenerateMaze();
        PlacePlayerAndGoal();
    }

    /// <summary>
    /// Creates and configures the tilemaps for the maze walls and floor.
    /// </summary>
    private void SetupTilemaps()
    {
        // Initialize wall tilemaps
        houseTilemaps = new Tilemap[houseWallTiles.Length];
        for (int i = 0; i < houseWallTiles.Length; i++)
        {
            GameObject tilemapObject = new GameObject($"HouseTilemap_{i + 1}");
            tilemapObject.transform.parent = this.transform;
            houseTilemaps[i] = tilemapObject.AddComponent<Tilemap>();
            var renderer = tilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingLayerName = "Foreground"; // Set sorting layer for rendering

            AddCollidersAndPhysics(tilemapObject); // Add colliders and physics to wall tilemaps
        }

        // Create a separate tilemap for the floor
        GameObject floorTilemapObject = new GameObject("FloorTilemap");
        floorTilemapObject.transform.parent = this.transform;
        floorTilemap = floorTilemapObject.AddComponent<Tilemap>();
        var floorRenderer = floorTilemapObject.AddComponent<TilemapRenderer>();
        floorRenderer.sortingLayerName = "Background"; // Set sorting layer for rendering
    }

    /// <summary>
    /// Adds colliders and physics components to a tilemap GameObject.
    /// </summary>
    private void AddCollidersAndPhysics(GameObject tilemapObject)
    {
        var tilemapCollider = tilemapObject.AddComponent<TilemapCollider2D>();
        var rb2D = tilemapObject.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Static;

        var compositeCollider = tilemapObject.AddComponent<CompositeCollider2D>();
        compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
        tilemapCollider.usedByComposite = true;
    }

    /// <summary>
    /// Generates a random maze using a depth-first search algorithm.
    /// </summary>
    private void GenerateMaze()
    {
        maze = new int[width, height];

        // Initialize maze grid with walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = (x == 0 || y == 0 || x == width - 1 || y == height - 1) ? 1 : 1; // Set boundaries as walls
            }
        }

        // Fill the floor tilemap with random floor tiles
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                floorTilemap.SetTile(tilePosition, GetRandomFloorTile());
            }
        }

        // Carve paths using depth-first search (DFS)
        Vector2Int start = new Vector2Int(1, 1);
        maze[start.x, start.y] = 0;

        var stack = new Stack<Vector2Int>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();
            List<Vector2Int> neighbors = GetValidNeighbors(current);

            if (neighbors.Count > 0)
            {
                stack.Push(current);

                Vector2Int chosenNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                Vector2Int between = current + (chosenNeighbor - current) / 2;

                maze[between.x, between.y] = 0;
                maze[chosenNeighbor.x, chosenNeighbor.y] = 0;

                stack.Push(chosenNeighbor);
            }
        }

        DrawMaze(); // Render the generated maze in the tilemaps
    }

    /// <summary>
    /// Draws the maze walls in the tilemaps based on the generated maze data.
    /// </summary>
    private void DrawMaze()
    {
        foreach (var tilemap in houseTilemaps)
        {
            tilemap.ClearAllTiles(); // Clear any existing tiles
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x - width / 2, y - height / 2, 0);
                if (maze[x, y] == 1) // If the cell is a wall
                {
                    int tilemapIndex = (x + y) % houseTilemaps.Length;
                    houseTilemaps[tilemapIndex].SetTile(tilePosition, houseWallTiles[tilemapIndex]);
                }
            }
        }
    }

    /// <summary>
    /// Gets valid neighbors for path carving during maze generation.
    /// </summary>
    private List<Vector2Int> GetValidNeighbors(Vector2Int current)
    {
        var validNeighbors = new List<Vector2Int>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = current + direction * 2;

            if (neighbor.x > 0 && neighbor.x < width - 1 && neighbor.y > 0 && neighbor.y < height - 1 && maze[neighbor.x, neighbor.y] == 1)
            {
                validNeighbors.Add(neighbor);
            }
        }

        return validNeighbors;
    }

    /// <summary>
    /// Places the player and goal at valid positions in the maze.
    /// </summary>
    private void PlacePlayerAndGoal()
    {
        do
        {
            playerStartPosition = FindRandomFloorPosition(-width / 2, width / 2, -height / 2, height / 2);
        } while (maze[playerStartPosition.x + width / 2, playerStartPosition.y + height / 2] != 0);

        player.transform.position = floorTilemap.CellToWorld(playerStartPosition) + new Vector3(0.5f, 0.5f, 0);
        goalPosition = FindFurthestFloorPosition(playerStartPosition);
        Instantiate(goalPrefab, floorTilemap.CellToWorld(goalPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    /// <summary>
    /// Finds a random floor position within the maze.
    /// </summary>
    private Vector3Int FindRandomFloorPosition(int minX, int maxX, int minY, int maxY)
    {
        Vector3Int position;
        do
        {
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            position = new Vector3Int(x, y, 0);
        } while (!IsFloorTile(floorTilemap.GetTile(position)));

        return position;
    }

    /// <summary>
    /// Finds the furthest floor position from a given start position.
    /// </summary>
    private Vector3Int FindFurthestFloorPosition(Vector3Int start)
    {
        var visited = new HashSet<Vector3Int>();
        var queue = new Queue<Vector3Int>();
        queue.Enqueue(start);
        Vector3Int furthestPosition = start;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            furthestPosition = current;

            foreach (var direction in new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right })
            {
                Vector3Int neighbor = current + direction;
                if (!visited.Contains(neighbor)
                    && IsFloorTile(floorTilemap.GetTile(neighbor))
                    && maze[neighbor.x + width / 2, neighbor.y + height / 2] == 0)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return furthestPosition;
    }

    /// <summary>
    /// Checks if a tile is a valid floor tile.
    /// </summary>
    private bool IsFloorTile(TileBase tile)
    {
        if (tile == null)
        {
            return false;
        }

        foreach (Tile ft in floorTiles)
        {
            if (tile == ft) 
            {
                return true; 
            }
        }

        return false;
    }

    /// <summary>
    /// Gets a random floor tile from the available floor tiles.
    /// </summary>
    private Tile GetRandomFloorTile()
    {
        return floorTiles[Random.Range(0, floorTiles.Length)];
    }
}


