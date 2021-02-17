using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterState {
    IDLE,
    MOVING,
    ATTACKING
}

public class Character : MonoBehaviour
{
    private CharacterState character_state = CharacterState.IDLE;
    private List<Vector2> cur_path;
    private int next_path_point_index;
    private Vector2 next_path_point;

    [SerializeField]
    private float max_hp;

    private float hp;

    private Image health_image;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject dead_body_prefab;

    private bool alive = true;

    private Animator animator;

    [SerializeField]
    private float body_disappear_time;

    [SerializeField]
    private bool delete_after_die = true;

    private float death_timer = 0.0f;

    [SerializeField]
    private float movement_speed;

    const float movement_epsilon = 0.05f;

    public CharacterState get_state() {
        return character_state;
    }

    private void move_along_path(int index) {
        next_path_point_index = index;
        next_path_point = cur_path[next_path_point_index]; 
        rb.velocity = (next_path_point - (Vector2)transform.position).normalized * movement_speed;
    }

    public void move_to(Vector2 pos) {
        cur_path = PathFinding.singleton().find_path(transform.position, pos);
        Debug.Log("paath from: " + transform.position + "to: " + pos);
        for (int i = cur_path.Count - 1; i >= 0; --i) {
            Debug.Log(cur_path[i]);
        }
        Debug.Log("-------------------------------------");
        if (cur_path == null || cur_path.Count == 0) {
            return;
        }
        animator.SetBool("moving", true);
        character_state = CharacterState.MOVING;
        move_along_path(cur_path.Count - 1);
    }

    public void stop_movement() {
        if (character_state == CharacterState.MOVING) {
            animator.SetBool("moving", false);
            character_state = CharacterState.IDLE;
        }
        rb.velocity = Vector2.zero;
    }

    public bool is_alive() {
        return alive;
    }

    void Awake() {
        hp = max_hp;
        health_image = transform.Find("HealthCanvas/Health").GetComponent<Image>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void apply_effect(Effect effect) {
        hp -= effect.damage;
        refresh_health_bar();
        if (hp <= 0.0f) {
            die();
        }
    }

    private void die() {
        for (int i = 0; i < transform.childCount; ++i) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        alive = false;
        transform.parent = null;
        if (rb != null) {
            rb.velocity = Vector2.zero;
        }
        animator.SetTrigger("die");
    }

    private void refresh_health_bar() {
        health_image.fillAmount = hp / max_hp;
    }

    public Vector2 get_expected_pos_at(float time) {
        return (Vector2)transform.position + rb.velocity * time;
    }

    private void handle_movement() {
        if (character_state != CharacterState.MOVING) {
            return;
        }
        if (((Vector2)transform.position - next_path_point).sqrMagnitude < movement_epsilon) {
            if (next_path_point_index > 0) {
                move_along_path(next_path_point_index - 1);
            } else {
                stop_movement();
                return;
            }
        }
        animator.SetFloat("x_dir", rb.velocity.x);
        animator.SetFloat("y_dir", rb.velocity.y);
    }

    private void handle_body_disappear() {
        if (!alive && death_timer < body_disappear_time) {
            death_timer += Time.deltaTime;
            if (death_timer >= body_disappear_time) {
                if (delete_after_die) {
                    GameObject.Destroy(gameObject);
                } else {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        handle_movement();
        handle_body_disappear();
    }
}
