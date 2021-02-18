using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttack : Attack
{
    private AttackAoE aoe_up;
    private AttackAoE aoe_down;
    private AttackAoE aoe_left;
    private AttackAoE aoe_right;

    void Awake() {
        base.Awake();
        aoe_up = transform.Find("AttackAoEUp").GetComponent<AttackAoE>();
        aoe_down = transform.Find("AttackAoEDown").GetComponent<AttackAoE>();
        aoe_left = transform.Find("AttackAoELeft").GetComponent<AttackAoE>();
        aoe_right = transform.Find("AttackAoERight").GetComponent<AttackAoE>();
    }

    protected override void instantiate_attack(Character target) {
        float x_dif = target.transform.position.x - transform.position.x;
        float y_dif = target.transform.position.y - transform.position.y;

        target.apply_effect(attack_effect);
    }
}
