using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro : MonoBehaviour
{
    private HashSet<Character> enemies_in_range = new HashSet<Character>(); 

    [SerializeField]
    private float attack_distance;

    private float sqr_attack_distance = 0.0f;

    private Character character;
    private Character cur_target;

    private const float meele_position_randomization = 0.1f;

    private Vector2 last_enemy_pos;
    private const float pos_diff_to_recalculate = 0.4f;

    void Awake() {
        character = transform.parent.GetComponent<Character>();
        sqr_attack_distance = attack_distance * attack_distance;
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
            character.set_state(CharacterState.IDLE);
        }
    }

    private void pick_new_target() {
        if (enemies_in_range.Count == 0) {
            return;
        }
        Character[] enemy_chars = new Character[enemies_in_range.Count];
        float[] weights = new float[enemies_in_range.Count];
        float total_weight = 0.0f;
        int index = 0;
        foreach (Character enemy_char in enemies_in_range) {
            weights[index] = 1.0f / Vector2.Distance(character.transform.position, enemy_char.transform.position);  
            total_weight += weights[index];
            enemy_chars[index] = enemy_char;
            ++index;
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
        // Debug.Log(character.get_state());
        if (character.get_state() != CharacterState.IDLE) {
            // if the character is already doing something else, return
            return;
        }

        pick_new_target();
        if (cur_target == null) {
            return;
        }

        character.set_state(CharacterState.ATTACKING);
        Debug.Log("attacking");
    }

    private void try_go_to_target() {
        if (character.get_state() != CharacterState.ATTACKING) {
            return;
        }
        if (((Vector2)cur_target.transform.position - (Vector2)character.transform.position).sqrMagnitude <= sqr_attack_distance) {
            return;
        }

        List<Vector2> least_populated_tiles = new List<Vector2>();
        int lowest_population = 0;
        for (int x_inc = -1; x_inc <= 1; ++x_inc) {
            for (int y_inc = -1; y_inc <= 1; ++y_inc) {
                if (x_inc == 0 && y_inc == 0) {
                    continue;
                }
                float x = cur_target.transform.position.x + 0.6f * x_inc * Character.meele_range + Random.Range(-meele_position_randomization, meele_position_randomization);
                float y = cur_target.transform.position.y + 0.6f * y_inc * Character.meele_range + Random.Range(-meele_position_randomization, meele_position_randomization);

                Vector2 new_pos = new Vector2(x, y); 

                if (!World.singleton().is_pos_passable(new_pos)) {
                    Debug.Log("is not pasableee");
                    continue;
                }

                int population_on_tile = World.singleton().get_population_on_tile(new_pos);
                if (population_on_tile < lowest_population || least_populated_tiles.Count == 0) {
                    lowest_population = population_on_tile; 
                    least_populated_tiles.Clear();
                }
                if (lowest_population == population_on_tile) {
                    least_populated_tiles.Add(new_pos);
                }
            }
        }
        Debug.Log("count: " + least_populated_tiles.Count);
        if (least_populated_tiles.Count == 0) {
            return;
        }
        if (lowest_population > 1) {
            return;
        }
        Vector2 chosen_pos = least_populated_tiles[Random.Range(0, least_populated_tiles.Count)];
        Debug.Log("chosen pos: " + chosen_pos);
        last_enemy_pos = cur_target.transform.position;
        character.move_to(chosen_pos);
    }

    private void stop_at_target() {
        if (character.get_state() != CharacterState.ATTACKING || character.get_action() != CharacterAction.MOVING) {
            return;
        }
        if (((Vector2)cur_target.transform.position - (Vector2)character.transform.position).sqrMagnitude <= sqr_attack_distance) {
            character.stop_movement();
            return;
        }
    }

    private void recalculate_path() {
        if (character.get_state() != CharacterState.ATTACKING || character.get_action() != CharacterAction.MOVING) {
            return;
        }
        if (((Vector2)cur_target.transform.position - last_enemy_pos).sqrMagnitude >= pos_diff_to_recalculate) {
            try_go_to_target();
            return;
        }
    }

    void Update()
    {
        if (!character.is_alive()) {
            return;
        }
        try_retarget();
        try_initiate_combat();
        try_go_to_target();
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
    }
}
