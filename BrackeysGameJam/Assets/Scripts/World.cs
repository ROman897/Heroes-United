using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileDebugInfo {
    public Vector2Int tile_coords;
    public Character[] characters;
}

public class World : MonoBehaviour
{
    public const float meele_range = 0.4f;
    public const float sqr_meele_range = meele_range * meele_range;

    [SerializeField]
    private float preferred_unit_distance;
    private float preferred_unit_distance_sqr = 0.0f;

    private static World instance;

    private HashSet<Vector2Int> passable_tiles = new HashSet<Vector2Int>();

    private Dictionary<Vector2Int, List<Character>> characters = new Dictionary<Vector2Int, List<Character>>();
    private Dictionary<Character, Vector2> active_movements = new Dictionary<Character, Vector2>();

    public TileDebugInfo[] debug_tiles;

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
        active_movements[character] = new_pos;
    }

    public void character_stopped(Vector2 new_pos, Character character) {
        Vector2Int old_dest_coords = world_to_coord(active_movements[character]);
        Vector2Int new_tile_coords = world_to_coord(new_pos);
        active_movements.Remove(character);
        if (old_dest_coords == new_tile_coords) {
            return;
        }
        _remove_character(old_dest_coords, character);
        _add_character(new_tile_coords, character);
    }

    private Vector2 get_future_char_pos(Character character) {
        Vector2 future_pos;
        if (!active_movements.TryGetValue(character, out future_pos)) {
            future_pos = character.transform.position;
        }
        return future_pos;
    }

    public int get_population_near_pos(Vector2 position, Character source_character) {
        Vector2Int tile_coords = world_to_coord(position);

        int count = 0;

        for (int x_inc = -1; x_inc <= 1; ++x_inc) {
            for (int y_inc = -1; y_inc <= 1; ++y_inc) {
                Vector2Int nearby_tile_coords = new Vector2Int(tile_coords.x + x_inc, tile_coords.y + y_inc);
                List<Character> characters_on_tile;
                if (characters.TryGetValue(nearby_tile_coords, out characters_on_tile)) {
                    foreach (Character character in characters_on_tile) {
                        if (source_character == character) {
                            continue;
                        }
                        if ((get_future_char_pos(character) - position).sqrMagnitude <= preferred_unit_distance_sqr) {
                            ++count;
                        }
                    }
                }
            }
        }
        return count;
    }

    void Update() {
        debug_tiles = new TileDebugInfo[characters.Count];
        int index = 0;
        foreach (var entry in characters) {
            debug_tiles[index] = new TileDebugInfo();
            debug_tiles[index].tile_coords = entry.Key;
            debug_tiles[index].characters = new Character[entry.Value.Count]; 
            for (int i = 0; i < entry.Value.Count; ++i) {
                debug_tiles[index].characters[i] = entry.Value[i];
            }
            ++index;
        }
    }

    void Awake() {
        floor_tilemap = GameObject.Find("FloorTilemap").GetComponent<Tilemap>();
        for (int x = floor_tilemap.cellBounds.min.x; x < floor_tilemap.cellBounds.max.x; ++x) {
            for (int y = floor_tilemap.cellBounds.min.y; y < floor_tilemap.cellBounds.max.y; ++y) {
                passable_tiles.Add(new Vector2Int(x, y));
            }
        }
        preferred_unit_distance_sqr = preferred_unit_distance * preferred_unit_distance;
    }

    public Vector2 coord_to_world(Vector2Int tile_coords) {
        Vector3 pos = floor_tilemap.CellToWorld((Vector3Int)tile_coords);
        return new Vector2(pos.x + 0.5f, pos.y + 0.5f);
    }

    public Vector2Int world_to_coord(Vector2 world_pos) {
        return (Vector2Int)floor_tilemap.WorldToCell(world_pos);
    }

    public bool is_tile_passable(Vector2Int tile_coords) {
        MyTile tile = floor_tilemap.GetTile((Vector3Int)tile_coords) as MyTile;
        if (tile == null) {
            return false;
        }
        return tile.passable;
    }

    public bool is_pos_passable(Vector2 world_pos) {
        Vector2Int tile_coords = (Vector2Int)floor_tilemap.WorldToCell(world_pos);
        return is_tile_passable(tile_coords);
    }


}
