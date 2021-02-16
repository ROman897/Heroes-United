using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    private static World instance;

    private HashSet<Vector2Int> passable_tiles = new HashSet<Vector2Int>();

    private Tilemap floor_tilemap;

    public static World singleton() {
        if (instance == null) {
            instance = GameObject.FindObjectOfType<World>();
            if (instance == null) {
                Debug.LogError("Instance of World is not found!");
            }
        }
        return instance;
    }

    void Awake() {
        floor_tilemap = GameObject.Find("FloorTilemap").GetComponent<Tilemap>();
        for (int x = floor_tilemap.cellBounds.min.x; x < floor_tilemap.cellBounds.max.x; ++x) {
            for (int y = floor_tilemap.cellBounds.min.y; y < floor_tilemap.cellBounds.max.y; ++y) {
                passable_tiles.Add(new Vector2Int(x, y));
            }
        }
    }

    public Vector2 coord_to_world(Vector2Int tile_coords) {
        Vector3 pos = floor_tilemap.CellToWorld((Vector3Int)tile_coords);
        return new Vector2(pos.x + 0.5f, pos.y + 0.5f);
    }

    public Vector2Int world_to_coord(Vector2 world_pos) {
        return (Vector2Int)floor_tilemap.WorldToCell(world_pos);
    }

    public bool is_tile_passable(Vector2Int tile_coords) {
        return true;
        return passable_tiles.Contains(tile_coords);
    }

    public bool is_pos_passable(Vector2 world_pos) {
        return true;
        Vector2Int tile_coords = (Vector2Int)floor_tilemap.WorldToCell(world_pos);
        return is_tile_passable(tile_coords);
    }


}
