using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMazeGenerator : MonoBehaviour
{
    const int HalfSizeMap = 11;
    const int Densitiy = 13;

    public Tile wallTile;
    public Tile floorTile;

    public GameObject player;
    public GameObject goalPrefab;

    private Tilemap wallTilemap;
    private Tilemap floorTilemap;

    private Vector3Int playerStartPosition;
    private Vector3Int goalPosition;

    private void Start()
    {
        SetupTilemaps();
        GenerateMaze();
        PlacePlayerAndGoal();
    }

    private void SetupTilemaps()
    {
        // Create separate Tilemap for walls
        GameObject wallTilemapObject = new GameObject("WallTilemap");
        wallTilemapObject.transform.parent = this.transform;
        wallTilemap = wallTilemapObject.AddComponent<Tilemap>();
        var wallRenderer = wallTilemapObject.AddComponent<TilemapRenderer>();
        wallRenderer.sortingLayerName = "Foreground"; // Assign sorting layer

        // Add Collider and Rigidbody to the wall tilemap
        AddCollidersAndPhysics(wallTilemapObject);

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
        rb2D.bodyType = RigidbodyType2D.Static; // Make it static
        rb2D.simulated = true; // Simulate the Rigidbody

        // Add CompositeCollider2D and configure it
        CompositeCollider2D compositeCollider = tilemapObject.AddComponent<CompositeCollider2D>();
        compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;

        // Link TilemapCollider2D to CompositeCollider2D
        tilemapCollider.usedByComposite = true;
    }

    private void GenerateMaze()
    {
        wallTilemap.ClearAllTiles();
        floorTilemap.ClearAllTiles();

        for (int x = -HalfSizeMap; x < HalfSizeMap; x++)
        {
            for (int y = -HalfSizeMap; y < HalfSizeMap; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (IsWall(x, y))
                {
                    wallTilemap.SetTile(tilePosition, wallTile);
                }
                else
                {
                    floorTilemap.SetTile(tilePosition, floorTile);
                }
            }
        }
    }

    private bool IsWall(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise(
            (x / (HalfSizeMap * 2.0f) * Densitiy),
            (y / (HalfSizeMap * 2.0f) * Densitiy)
        );

        return rawPerlin > 0.5f;
    }

    private void PlacePlayerAndGoal()
    {
        // Ensure player starts at a valid position on the floor tilemap
        playerStartPosition = FindRandomFloorPosition();
        player.transform.position = floorTilemap.CellToWorld(playerStartPosition) + new Vector3(0.5f, 0.5f, 0);

        // Ensure goal is placed at a valid position on the floor tilemap and is reachable
        do
        {
            goalPosition = FindRandomFloorPosition();
        } while (!IsPathReachable(playerStartPosition, goalPosition));

        Instantiate(goalPrefab, floorTilemap.CellToWorld(goalPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    private Vector3Int FindRandomFloorPosition()
    {
        int x, y;
        do
        {
            x = Random.Range(-HalfSizeMap, HalfSizeMap);
            y = Random.Range(-HalfSizeMap, HalfSizeMap);
        } while (floorTilemap.GetTile(new Vector3Int(x, y, 0)) != floorTile);

        return new Vector3Int(x, y, 0);
    }

    private bool IsPathReachable(Vector3Int start, Vector3Int end)
    {
        // Using breadth-first search
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
                if (!visited.Contains(neighbor) && floorTilemap.GetTile(neighbor) == floorTile)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return false;
    }
}
