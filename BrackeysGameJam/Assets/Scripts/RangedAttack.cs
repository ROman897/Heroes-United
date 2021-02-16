using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField]
    private GameObject projectile_prefab;

    private Transform spawn_point_up;
    private Transform spawn_point_down;
    private Transform spawn_point_left;
    private Transform spawn_point_right;

    void Awake() {
        base.Awake();
        spawn_point_up = transform.Find("ProjectileSpawnUp");
        spawn_point_down = transform.Find("ProjectileSpawnDown");
        spawn_point_left = transform.Find("ProjectileSpawnLeft");
        spawn_point_right = transform.Find("ProjectileSpawnRight");
    }

    protected override void instantiate_attack(GameObject target) {
        Transform target_char = target.transform.parent;
        float x_dif = target_char.position.x - transform.position.x;
        float y_dif = target_char.position.y - transform.position.y;

        Transform spawn_point;

        if (Mathf.Abs(x_dif) > Mathf.Abs(y_dif)) {
            if (x_dif > 0) {
                spawn_point = spawn_point_right;
            } else {
                spawn_point = spawn_point_left;
            }
        } else {
            if (y_dif > 0) {
                spawn_point = spawn_point_up;
            } else {
                spawn_point = spawn_point_down;
            }
        }
        GameObject projectile = GameObject.Instantiate(projectile_prefab, spawn_point.position, Quaternion.identity);
        if (gameObject.tag == "Hero") {
            projectile.layer = LayerMask.NameToLayer("PlayerAttackRange");
        } else {
            projectile.layer = LayerMask.NameToLayer("EnemyAttackRange");
        }
        projectile.GetComponent<Projectile>().set_target(target_char.gameObject);
    }

}
