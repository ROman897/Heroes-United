using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro : MonoBehaviour
{
    private HashSet<Character> enemies_in_range = new HashSet<Character>(); 

    private Character character;
    public Character cur_target;

    private const float meele_position_randomization = 0.07f;

    private Vector2 last_enemy_pos;

    public Vector2 debug_target_pos;
    public Vector2Int debug_target_coords;

    private const float epsilon = 0.05f;

    [SerializeField]
    private bool is_ranged;

    public Character get_cur_target() {
        return cur_target;
    }

    void Awake() {
        character = transform.parent.GetComponent<Character>();
    }

    public bool can_attack() {
        return cur_target != null && ((Vector2)cur_target.transform.position - (Vector2)transform.position).sqrMagnitude <= World.sqr_meele_range;
    }

    private void try_retarget() {
        if (character.get_state() != CharacterState.ATTACKING) {
            // if the character is already doing something else, return
            return;
        }
        if (cur_target != null) {
            return;
        }
        pick_new_target();

        if (cur_target == null) {
            Debug.Log(transform.parent.gameObject.name + " cancelling combat no enemy found");
            character.set_state(CharacterState.IDLE);
        } else {
            Debug.Log(transform.parent.gameObject.name + " retargeting to: " + cur_target.gameObject.name);
            try_go_to_target();
            stop_at_target();
        }
    }

    private void pick_new_target() {
        if (enemies_in_range.Count == 0) {
            Debug.Log(transform.parent.gameObject.name + " cannot pick new target, nobody nearby");
            return;
        }
        Character[] enemy_chars = new Character[enemies_in_range.Count];
        float[] weights = new float[enemies_in_range.Count];
        float total_weight = 0.0f;
        int index = 0;

        List<Character> characters_in_direct_range = new List<Character>();

        foreach (Character enemy_char in enemies_in_range) {
            float sqr_distance = ((Vector2)character.transform.position - (Vector2)enemy_char.transform.position).sqrMagnitude;
            if (sqr_distance <= World.sqr_meele_range) {
                characters_in_direct_range.Add(enemy_char);
            }
            weights[index] = 1.0f / sqr_distance;  
            total_weight += weights[index];
            enemy_chars[index] = enemy_char;
            ++index;
        }
        if (characters_in_direct_range.Count > 0) {
            cur_target = characters_in_direct_range[Random.Range(0, characters_in_direct_range.Count - 1)];
            return;
        }
        float[] probs = new float[enemies_in_range.Count];
        for (int i = 0; i < enemies_in_range.Count; ++i) {
            probs[i] = weights[i] / total_weight;
        }

        float rand = Random.Range(0.0f, 1.0f);
        for (int i = 0; i < enemies_in_range.Count - 1; ++i) {
            if (probs[i] > rand) {
                cur_target = enemy_chars[i];
                return;
            }
            rand -= probs[i];
        }
        cur_target = enemy_chars[enemies_in_range.Count - 1];
    }

    private void try_initiate_combat() {
        if (character.get_state() != CharacterState.IDLE && character.get_state() != CharacterState.COMMAND_MOVE) {
            // if the character is already doing something else, return
            return;
        }

        pick_new_target();
        if (cur_target == null) {
            // Debug.Log("cannot initiate combaat");
            return;
        }

        character.set_state(CharacterState.ATTACKING);
        try_go_to_target();
    }

    private void try_go_to_target() {
        if (character.get_state() != CharacterState.ATTACKING) {
            return;
        }
        List<Vector2> free_tiles = new List<Vector2>();

        float min_sqr_dist = 1000.0f;

        for (int x_inc = -1; x_inc <= 1; ++x_inc) {
            for (int y_inc = -1; y_inc <= 1; ++y_inc) {
                if (x_inc == 0 && y_inc == 0) {
                    continue;
                }

                Vector2 inc = new Vector2(x_inc, y_inc); 
                inc = inc.normalized * 0.95f * World.meele_range;

                Vector2 new_pos = (Vector2)cur_target.transform.position + inc; 

                if (!World.singleton().is_pos_passable(new_pos)) {
                    continue;
                }

                if (!can_stop_at_pos(new_pos)) {
                    continue;
                }

                float sqr_dist = ((Vector2)cur_target.transform.position - new_pos).sqrMagnitude;
                min_sqr_dist = Mathf.Min(min_sqr_dist, sqr_dist);

                free_tiles.Add(new_pos);
            }
        }
        if (free_tiles.Count == 0) {
            Debug.Log("no tiles available");
            return;
        }

        List<Vector2> close_tiles = new List<Vector2>();
        foreach (Vector2 free_tile in free_tiles) {
            if (((Vector2)cur_target.transform.position - free_tile).sqrMagnitude - min_sqr_dist < epsilon) {
                close_tiles.Add(free_tile);
            }
        }

        Vector2 chosen_pos = free_tiles[Random.Range(0, close_tiles.Count)];
        last_enemy_pos = cur_target.transform.position;
        character.move_to(chosen_pos);
        debug_target_pos = chosen_pos;
        debug_target_coords = World.singleton().world_to_coord(chosen_pos); 
        Debug.Log(transform.parent.gameObject.name + "moving to: " + chosen_pos);
    }

    private bool can_stop_at_pos(Vector2 pos) {
        return World.singleton().get_population_near_pos(pos, character) == 0;
    }

    private void stop_at_target() {
        if (character.get_state() != CharacterState.ATTACKING || character.get_action() != CharacterAction.MOVING) {
            return;
        }
        if (((Vector2)cur_target.transform.position - (Vector2)character.transform.position).sqrMagnitude > World.sqr_meele_range) {
            return;
        }
        if (!can_stop_at_pos(transform.position)) {
            return;
        }
        Debug.Log(transform.parent.gameObject.name + " stopping at: " + transform.position + "because I am in range for attack");
        character.stop_movement();
    }

    private void recalculate_path() {
        if (character.get_state() != CharacterState.ATTACKING) {
            return;
        }

        if (character.get_action() == CharacterAction.IDLE) {
            if (((Vector2)cur_target.transform.position - (Vector2)transform.position).sqrMagnitude > 0.99f * World.sqr_meele_range) {
                try_go_to_target();
                return;
            }
        }

        if (character.get_action() != CharacterAction.MOVING) {
            return;
        }

        // Debug.Log(transform.parent.gameObject.name + " recalculate distance: " + ((Vector2)cur_target.transform.position - last_enemy_pos).sqrMagnitude + "out of: " + 0.9f * World.sqr_meele_range);
        if (((Vector2)cur_target.transform.position - last_enemy_pos).sqrMagnitude > 0.99f * World.sqr_meele_range) {
            try_go_to_target();
            return;
        }
    }

    void Update()
    {
        if (!character.is_alive()) {
            return;
        }
        try_initiate_combat();
        try_retarget();
        stop_at_target();
        recalculate_path();
    }

    void OnTriggerEnter2D(Collider2D other) {
        enemies_in_range.Add(other.transform.parent.GetComponent<Character>());
    }

    void OnTriggerExit2D(Collider2D other) {
        Character enemy_char = other.transform.parent.GetComponent<Character>();
        if (cur_target == enemy_char) {
            cur_target = null;
        }
        enemies_in_range.Remove(enemy_char);
    }
}
