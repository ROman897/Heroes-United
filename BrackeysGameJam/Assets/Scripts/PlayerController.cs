﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movement_speed;

    private Rigidbody2D rigidbody;

    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float y_movement = 0;
        if (Input.GetKey("w")) {
            y_movement = 1;
        } else {
            if (Input.GetKey("s")) {
                y_movement = -1;
            }
        }

        float x_movement = 0;
        if (Input.GetKey("d")) {
            x_movement = 1;

        }
        else {
            if (Input.GetKey("a")) {
                x_movement = -1;
            }
        }

        Vector2 movement = new Vector2(x_movement, y_movement);
        if (movement != Vector2.zero) {
            movement = movement.normalized * movement_speed;
        }

        rigidbody.velocity = movement;
    }
}
