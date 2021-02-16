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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
