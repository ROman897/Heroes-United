using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private HashSet<GameObject> enemies_in_range = new HashSet<GameObject>();

    private GameObject cur_target;

    [SerializeField]
    private float attack_cooldown;

    private float cur_attack_cooldown = 0.0f;

    private Animator animator;

    void Awake() {
        CollisionTrigger attack_trigger = transform.Find("AttackTrigger").GetComponent<CollisionTrigger>();
        attack_trigger.on_trigger_enter.AddListener(add_enemy);
        attack_trigger.on_trigger_exit.AddListener(remove_enemy);
        animator = GetComponent<Animator>();
    }

    private void add_enemy(GameObject enemy) {
        enemies_in_range.Add(enemy);
        pick_target();
    }

    private void remove_enemy(GameObject enemy) {
        enemies_in_range.Remove(enemy);
        pick_target();
    }

    private void pick_target() {
        float min_dist = 0;
        GameObject target = null;
        foreach (GameObject possible_target in enemies_in_range) {
            float dist = Vector2.Distance(transform.position, possible_target.transform.position);
            if (dist < min_dist || target == null) {
                min_dist = dist;
                target = possible_target;
            }
        }
        cur_target = target;
    }

    private string get_attack_direction(GameObject target) {
        float x_dif = target.transform.position.x - transform.position.x;
        float y_dif = target.transform.position.y - transform.position.y;
        if (Mathf.Abs(x_dif) > Mathf.Abs(y_dif)) {
            if (x_dif > 0) {
                return "right";
            }
            return "left";
        } else {
            if (y_dif > 0) {
                return "up";
            }
            return "down";
        }
    }

    private void attack() {
        string trigger_name = "attack_" + get_attack_direction(cur_target); 
        animator.SetTrigger(trigger_name);
        cur_attack_cooldown = attack_cooldown;
    }

    void Update() {
        if (cur_attack_cooldown > 0.0f) {
            cur_attack_cooldown -= Time.deltaTime;
            return;
        }
        if (cur_target != null) {
            attack();
        }
    }
}
