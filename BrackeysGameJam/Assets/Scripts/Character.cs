using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterState {
    IDLE,
    COMMAND_MOVE,
    ATTACKING
}

public enum CharacterAction {
    IDLE,
    MOVING,
    SPECIAL
}

public class Character : MonoBehaviour
{
    public const float meele_range = 0.4f;

    private CharacterState character_state = CharacterState.IDLE;
    private CharacterAction character_action = CharacterAction.IDLE;

    private List<Vector2> cur_path;
    private int next_path_point_index;
    private Vector2 next_path_point;

    [SerializeField]
    private float max_hp;
    private float hp;

    private Image health_image;

    [SerializeField]
    private Rigidbody2D rb;

    private bool alive = true;

    private Animator animator;

    [SerializeField]
    private float body_disappear_time;

    [SerializeField]
    private bool delete_after_die = true;

    private float death_timer = 0.0f;

    [SerializeField]
    private float movement_speed;

    const float movement_epsilon = 0.0001f;

    public CharacterState get_state() {
        return character_state;
    }

    public CharacterAction get_action() {
        return character_action;
    }

    public void set_state(CharacterState character_state) {
        this.character_state = character_state;
    }

    public void set_action(CharacterAction character_action) {
        this.character_action = character_action;
    }

    private void move_along_path(int index) {
        next_path_point_index = index;
        next_path_point = cur_path[next_path_point_index]; 
        rb.velocity = (next_path_point - (Vector2)transform.position).normalized * movement_speed;
    }

    public void move_to(Vector2 pos) {
        stop_movement();
        cur_path = PathFinding.singleton().find_path(transform.position, pos);
        if (cur_path == null || cur_path.Count == 0) {
            return;
        }

        World.singleton().character_started_moving(transform.position, pos, this);

        for (int i = cur_path.Count - 1; i >= 0; --i) {
            Debug.Log(i + "th position: " + cur_path[i].x + " , " + cur_path[i].y);
        }

        animator.SetBool("moving", true);
        character_action = CharacterAction.MOVING;
        move_along_path(cur_path.Count - 1);
        Debug.Log("moving to " + pos.x + " " + pos.y);
    }

    public void stop_movement() {
        if (character_action == CharacterAction.MOVING) {
            animator.SetBool("moving", false);
            character_action = CharacterAction.IDLE;
            World.singleton().character_stopped(transform.position, this);
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

    void Start() {
        World.singleton().add_character(transform.position, this);
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
        stop_movement();
        animator.SetTrigger("die");
        World.singleton().remove_character(transform.position, this);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void refresh_health_bar() {
        health_image.fillAmount = hp / max_hp;
    }

    public Vector2 get_expected_pos_at(float time) {
        return (Vector2)transform.position + rb.velocity * time;
    }

    private void handle_movement() {
        if (character_action != CharacterAction.MOVING) {
            return;
        }
        if (((Vector2)transform.position - next_path_point).sqrMagnitude < movement_epsilon) {
            if (next_path_point_index > 0) {
                move_along_path(next_path_point_index - 1);
            } else {
                Debug.Log("reached end of movement at pos: " + transform.position.x + " , " + transform.position.y);
                Debug.Log("last pos was: " + next_path_point.x + " , " + next_path_point.y);
                stop_movement();
                return;
            }
        }
        animator.SetFloat("x_dir", rb.velocity.x);
        animator.SetFloat("y_dir", rb.velocity.y * 0.1f);
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

    private void set_z_pos() {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    void Update()
    {
        handle_movement();
        handle_body_disappear();
        set_z_pos();
    }
}
