using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    private static World instance;

    private HashSet<Vector2Int> passable_tiles = new HashSet<Vector2Int>();

    private Dictionary<Vector2Int, List<Character>> characters = new Dictionary<Vector2Int, List<Character>>();
    private Dictionary<Character, Vector2Int> active_movements = new Dictionary<Character, Vector2Int>();

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

    public void add_character(Vector2 position, Character character) {
        Vector2Int tile_coords = world_to_coord(position);
        _add_character(tile_coords, character);
    }

    private void _add_character(Vector2Int tile_coords, Character character) {
        List<Character> characters_on_tile;
        if (!characters.TryGetValue(tile_coords, out characters_on_tile)) {
            characters.Add(tile_coords, characters_on_tile = new List<Character>());
        }
        characters_on_tile.Add(character);
    }

    public void remove_character(Vector2 position, Character character) {
        Vector2Int tile_coords = world_to_coord(position);
        _remove_character(tile_coords, character);
    }

    private void _remove_character(Vector2Int tile_coords, Character character) {
        characters[tile_coords].Remove(character);
    }

    public void character_started_moving(Vector2 old_pos, Vector2 new_pos, Character character) {
        remove_character(old_pos, character);
        add_character(new_pos, character);
        Vector2Int new_tile_coords = world_to_coord(new_pos);
        active_movements[character] = new_tile_coords;
    }

    public void character_stopped(Vector2 new_pos, Character character) {
        Vector2Int old_dest_coords = active_movements[character];
        Vector2Int new_tile_coords = world_to_coord(new_pos);
        if (old_dest_coords == new_tile_coords) {
            return;
        }
        _remove_character(old_dest_coords, character);
        _add_character(new_tile_coords, character);
        active_movements.Remove(character);
    }

    public int get_population_on_tile(Vector2 position) {
        Vector2Int tile_coords = world_to_coord(position);
        List<Character> characters_on_tile;
        if (characters.TryGetValue(tile_coords, out characters_on_tile)) {
            return characters_on_tile.Count;
        }
        return 0;
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
