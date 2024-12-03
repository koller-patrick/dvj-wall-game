using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMazeGenerator : MonoBehaviour
{
    const int HalfSizeMap = 11;
    const int Densitiy = 13;

    public Tile[] houseWallTiles;
    public Tile floorTile;

    public GameObject player;
    public GameObject goalPrefab;

    private Tilemap[] houseTilemaps;
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
        houseTilemaps = new Tilemap[houseWallTiles.Length];
        for (int i = 0; i < houseWallTiles.Length; i++)
        {
            GameObject houseTilemapObject = new GameObject($"HouseTilemap_{i + 1}");
            houseTilemapObject.transform.parent = this.transform;
            houseTilemaps[i] = houseTilemapObject.AddComponent<Tilemap>();
            var renderer = houseTilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingLayerName = "Foreground";

            // Add Collider and Rigidbody to the wall tilemap
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
        // Clear all tiles
        foreach (var houseTilemap in houseTilemaps)
        {
            houseTilemap.ClearAllTiles();
        }
        floorTilemap.ClearAllTiles();

        for (int x = -HalfSizeMap; x < HalfSizeMap; x++)
        {
            for (int y = -HalfSizeMap; y < HalfSizeMap; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (IsWall(x, y))
                {
                    int houseIndex = (Mathf.Abs(x + y) % houseTilemaps.Length); // Alternate between house tilemaps
                    houseTilemaps[houseIndex].SetTile(tilePosition, houseWallTiles[houseIndex]);
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
        // Player starts in the bottom-left corner of the maze
        playerStartPosition = FindRandomFloorPosition(-HalfSizeMap / 2, -HalfSizeMap, -HalfSizeMap / 2, -HalfSizeMap);
        player.transform.position = floorTilemap.CellToWorld(playerStartPosition) + new Vector3(0.5f, 0.5f, 0);

        // Goal is placed in the top-right corner of the maze
        do
        {
            goalPosition = FindRandomFloorPosition(HalfSizeMap / 2, HalfSizeMap, HalfSizeMap / 2, HalfSizeMap);
        } while (!IsPathReachable(playerStartPosition, goalPosition));

        // Instantiate the goal object at the valid position
        GameObject goal = Instantiate(goalPrefab, floorTilemap.CellToWorld(goalPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    private Vector3Int FindRandomFloorPosition(int minX, int maxX, int minY, int maxY)
    {
        int x, y;
        Vector3Int position;
        do
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            position = new Vector3Int(x, y, 0);
        } while (floorTilemap.GetTile(position) != floorTile); // Ensure the position is on the floor tilemap

        return position;
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
