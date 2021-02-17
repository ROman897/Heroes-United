using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect {
    public float damage;
}

public abstract class Attack : MonoBehaviour
{
    [SerializeField]
    protected Effect attack_effect;

    private HashSet<GameObject> enemies_in_range = new HashSet<GameObject>();

    private GameObject cur_target;

    [SerializeField]
    private float attack_cooldown;

    private float cur_attack_cooldown = 0.0f;

    private Animator animator;

    private Character character;

    public bool is_attacking() {
        return enemies_in_range.Count > 0;
    }

    protected void Awake() {
        CollisionTrigger attack_trigger = transform.Find("AttackTrigger").GetComponent<CollisionTrigger>();
        attack_trigger.on_trigger_enter.AddListener(add_enemy);
        attack_trigger.on_trigger_exit.AddListener(remove_enemy);
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
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

    private void attack() {
        animator.SetFloat("x_attack", cur_target.transform.position.x - transform.position.x);
        animator.SetFloat("y_attack", cur_target.transform.position.y - transform.position.y);

        animator.SetFloat("x_dir", cur_target.transform.position.x - transform.position.x);
        animator.SetFloat("y_dir", cur_target.transform.position.y - transform.position.y);

        animator.SetTrigger("attack");
        cur_attack_cooldown = attack_cooldown;

        instantiate_attack(cur_target);
    }

    protected abstract void instantiate_attack(GameObject target);

    private void cancel_attack_if_no_enemies() {
        if (character.get_state() != CharacterState.ATTACKING) {
            return;
        }
        if (cur_target == null) {
            character.set_state(CharacterState.IDLE);
        }
    }

    private void try_initiate_attacking() {
        if (character.get_state() != CharacterState.IDLE || cur_target == null) {
            return;
        }
        Debug.Log("initiate attaaaack");
        character.set_state(CharacterState.ATTACKING);
    }

    private void try_attack() {
        if (cur_attack_cooldown > 0.0f) {
            cur_attack_cooldown -= Time.deltaTime;
            return;
        }

        if (character.get_state() != CharacterState.ATTACKING) {
            return;
        }
        if (character.get_action() != CharacterAction.IDLE) {
            return;
        }

        if (cur_target != null) {
            attack();
        }
    }

    void Update() {
        if (!character.is_alive()) {
            return;
        }

        // cancel_attack_if_no_enemies();
        // try_initiate_attacking();
        try_attack();

    }
}
