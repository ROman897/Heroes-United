using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
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

    public bool is_alive() {
        return alive;
    }

    void Awake() {
        hp = max_hp;
        health_image = transform.Find("HealthCanvas/Health").GetComponent<Image>();
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("die");
    }

    private void refresh_health_bar() {
        health_image.fillAmount = hp / max_hp;
    }

    public Vector2 get_expected_pos_at(float time) {
        return transform.position;
        // return (Vector2)transform.position + rb.velocity * time;
    }

    void Update()
    {
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
}
