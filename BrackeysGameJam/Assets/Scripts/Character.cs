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

    void Awake() {
        hp = max_hp;
        health_image = transform.Find("HealthCanvas/Health").GetComponent<Image>();
    }

    public void apply_effect(Effect effect) {
        hp -= effect.damage;
        refresh_health_bar();
        Debug.Log("apply effect hp left: " + hp);
    }

    private void refresh_health_bar() {
        health_image.fillAmount = hp / max_hp;
    }

    public Vector2 get_expected_pos_at(float time) {
        return transform.position;
        // return (Vector2)transform.position + rb.velocity * time;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
