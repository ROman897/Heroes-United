using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultProjectile : Projectile
{
    private Vector2 static_target;
    private Vector2 current_speed;

    [SerializeField]
    private float gravity;
    [SerializeField]
    private float travel_time;

    private float travel_timer = 0.0f;

    [SerializeField]
    private bool detonate_on_miss = false;

    public override void set_target(GameObject target) {
        static_target = target.GetComponent<Character>().get_expected_pos_at(travel_time);
        set_start_speed();
    }

    private void set_start_speed() {
        float x = (static_target.x - transform.position.x) / travel_time;
        float y_dif = (static_target.y - transform.position.y);
        float y = 0.5f * gravity * travel_time + y_dif / travel_time;
        current_speed = new Vector2(x, y);
    }

    public void Update() {
        transform.position = (Vector2)transform.position + (Vector2)current_speed * Time.deltaTime;
        current_speed = new Vector2(current_speed.x, current_speed.y - gravity * Time.deltaTime);

        if (rotate) {
            rotate_towards(current_speed);
        }

        travel_timer += Time.deltaTime;

        if (travel_timer >= travel_time * 1.25f) {
            if (detonate_on_miss) {
                // GetComponent<ProjectileEffect>().apply_and_destroy();
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
