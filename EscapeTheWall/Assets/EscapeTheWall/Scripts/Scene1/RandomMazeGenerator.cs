using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMazeGenerator : MonoBehaviour
{
    public Tile[] houseWallTiles;
    public Tile[] floorTiles;

    public GameObject player;
    public GameObject goalPrefab;

    private Tilemap[] houseTilemaps;
    private Tilemap floorTilemap;

    private Vector3Int playerStartPosition;
    private Vector3Int goalPosition;

    private int[,] maze;
    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private int width;
    private int height;

    void Start()
    {
        CalculateMazeSizeFromCamera();
        SetupTilemaps();
        GenerateMaze();
        PlacePlayerAndGoal();
    }

    private void CalculateMazeSizeFromCamera()
    {
        // Calculate the maze size based on the camera view
        Camera mainCamera = Camera.main;
        float aspectRatio = mainCamera.aspect;
        float cameraHeight = mainCamera.orthographicSize * 2;

        width = Mathf.RoundToInt(cameraHeight * aspectRatio);
        height = Mathf.RoundToInt(cameraHeight);

        width -= 10;
        height -= 4;

        // // Ensure width and height are odd
        // if (width % 2 == 0) width++;
        // if (height % 2 == 0) height++;
    }

    private void SetupTilemaps()
    {
        houseTilemaps = new Tilemap[houseWallTiles.Length];
        for (int i = 0; i < houseWallTiles.Length; i++)
        {
            GameObject houseTilemapObject = new GameObject($"HouseTilemap_{i + 1}");
            houseTilemapObject.transform.parent = this.transform;
            houseTilemaps[i] = houseTilemapObject.AddComponent<Tilemap>();
            var renderer = houseTilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingLayerName = "Foreground";

            // Add Collider and Physics
            AddCollidersAndPhysics(houseTilemapObject);
        }

        // Create separate Tilemap for floors
        GameObject floorTilemapObject = new GameObject("FloorTilemap");
        floorTilemapObject.transform.parent = this.transform;
        floorTilemap = floorTilemapObject.AddComponent<Tilemap>();
        var floorRenderer = floorTilemapObject.AddComponent<TilemapRenderer>();
        floorRenderer.sortingLayerName = "Background"; // Assign sorting layer
    }

    private void AddCollidersAndPhysics(GameObject tilemapObject)
    {
        // Add TilemapCollider2D
        TilemapCollider2D tilemapCollider = tilemapObject.AddComponent<TilemapCollider2D>();

        // Add Rigidbody2D
        Rigidbody2D rb2D = tilemapObject.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Static;
        rb2D.simulated = true;

        // Add CompositeCollider2D
        CompositeCollider2D compositeCollider = tilemapObject.AddComponent<CompositeCollider2D>();
        compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;

        // Link TilemapCollider2D to CompositeCollider2D
        tilemapCollider.usedByComposite = true;
    }

    private void GenerateMaze()
    {
        // Initialize the maze grid
        maze = new int[width, height];

          // Set boundaries as walls
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
            {
                maze[x, y] = 1; // Outer boundary walls
            }
            else
            {
                maze[x, y] = 1; // Start interiors as walls (carving will remove them)
            }
        }
    }

        // Fill the entire tilemap with random floor tiles
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                floorTilemap.SetTile(tilePosition, GetRandomFloorTile());
            }
        }

        // Carve out paths using DFS
        Vector2Int start = new Vector2Int(1, 1);
        maze[start.x, start.y] = 0;

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();
            List<Vector2Int> neighbors = GetValidNeighbors(current);

            if (neighbors.Count > 0)
            {
                stack.Push(current);

                // Choose a random neighbor
                Vector2Int chosenNeighbor = neighbors[Random.Range(0, neighbors.Count)];

                // Remove the wall between current and chosenNeighbor
                Vector2Int between = current + (chosenNeighbor - current) / 2;
                maze[between.x, between.y] = 0;
                maze[chosenNeighbor.x, chosenNeighbor.y] = 0;

                stack.Push(chosenNeighbor);
            }
        }

        // Draw the maze with walls
        DrawMaze();
    }

    private void DrawMaze()
    {
        // Clear all tiles in wall tilemaps
        foreach (var houseTilemap in houseTilemaps)
        {
            houseTilemap.ClearAllTiles();
        }

        // Draw walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x - width / 2, y - height / 2, 0);
                int mazeValue = maze[x, y];

                if (mazeValue == 1)
                {
                    int houseIndex = (Mathf.Abs(x + y) % houseTilemaps.Length);
                    houseTilemaps[houseIndex].SetTile(tilePosition, houseWallTiles[houseIndex]);
                }
            }
        }
    }

    private List<Vector2Int> GetValidNeighbors(Vector2Int current)
    {
        List<Vector2Int> validNeighbors = new List<Vector2Int>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = current + direction * 2;

            if (neighbor.x > 0 && neighbor.x < width - 1 && neighbor.y > 0 && neighbor.y < height - 1)
            {
                if (maze[neighbor.x, neighbor.y] == 1)
                {
                    validNeighbors.Add(neighbor);
                }
            }
        }

        return validNeighbors;
    }

    private void PlacePlayerAndGoal()
    {
        // Find a valid player start position on the floor
        do
        {
            playerStartPosition = FindRandomFloorPosition(-width / 2, width / 2, -height / 2, height / 2);
        } while (maze[playerStartPosition.x + width / 2, playerStartPosition.y + height / 2] != 0);

        player.transform.position = floorTilemap.CellToWorld(playerStartPosition) + new Vector3(0.5f, 0.5f, 0);

        // Find furthest floor position using BFS
        goalPosition = FindFurthestFloorPosition(playerStartPosition);

        // Instantiate the goal
        Instantiate(goalPrefab, floorTilemap.CellToWorld(goalPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    private Vector3Int FindFurthestFloorPosition(Vector3Int start)
    {
        // BFS to find the farthest floor position
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

    private bool IsPathReachable(Vector3Int start, Vector3Int end)
    {
        // BFS to check reachability
        var visited = new HashSet<Vector3Int>();
        var queue = new Queue<Vector3Int>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == end)
            {
                return true;
            }

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

        return false;
    }

    private bool IsFloorTile(TileBase tile)
    {
        if (tile == null) return false;
        foreach (var ft in floorTiles)
        {
            if (tile == ft) return true;
        }
        return false;
    }

    private Tile GetRandomFloorTile()
    {
        return floorTiles[Random.Range(0, floorTiles.Length)];
    }
}

