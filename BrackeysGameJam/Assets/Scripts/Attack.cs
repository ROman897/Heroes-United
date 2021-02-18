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

    private Aggro aggro;

    [SerializeField]
    private float attack_cooldown;

    private float cur_attack_cooldown = 0.0f;

    private Animator animator;

    private Character character;

    protected void Awake() {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        aggro = transform.Find("AggroRange").GetComponent<Aggro>();
    }

    private void attack() {
        Character cur_target = aggro.get_cur_target();
        animator.SetFloat("x_attack", cur_target.transform.position.x - transform.position.x);
        animator.SetFloat("y_attack", cur_target.transform.position.y - transform.position.y);

        animator.SetFloat("x_dir", cur_target.transform.position.x - transform.position.x);
        animator.SetFloat("y_dir", cur_target.transform.position.y - transform.position.y);

        animator.SetTrigger("attack");
        cur_attack_cooldown = attack_cooldown;

        instantiate_attack(aggro.get_cur_target());
    }

    protected abstract void instantiate_attack(Character target);

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

        if (aggro.get_cur_target() != null) {
            attack();
        }
    }

    void Update() {
        if (!character.is_alive()) {
            return;
        }

        try_attack();

    }
}
