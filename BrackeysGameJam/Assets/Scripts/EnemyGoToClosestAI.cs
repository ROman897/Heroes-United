using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoToClosestAI : MonoBehaviour
{
    private Character cur_target;

    private Attack attack;
    private Character character;

    void Awake() {
        attack = GetComponent<Attack>();
        character = GetComponent<Character>();
    }

    void Update()
    {
        if (!character.is_alive()) {
            return;
        }
        if (cur_target != null && !cur_target.is_alive()) {
            cur_target = null;
        }
        if (!attack.is_attacking() && cur_target == null) {
            find_target();
        }
        if (!attack.is_attacking() && cur_target != null) {
            Debug.Log("ai move too");
            character.move_to(cur_target.transform.position);
        }
        if (attack.is_attacking()) {
            character.stop_movement();
        }
        if (cur_target == null) {
            character.stop_movement();
        }
    }

    private void find_target() {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");

        Character chosen_target = null;
        float min_dist = 0;

        foreach (GameObject hero_go in heroes) {
            Character hero_char = hero_go.GetComponent<Character>();
            if (!hero_char.is_alive()) {
                continue;
            }
            float dist = Vector2.Distance(hero_go.transform.position, transform.position);
            if (chosen_target == null || dist < min_dist) {
                min_dist = dist;
                chosen_target = hero_char;
            }
        }
        cur_target = chosen_target;
    }
}
