using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movement_speed;
    [SerializeField]
    private float rotation_speed;
    [SerializeField]
    private bool FollowsMousePos;
    [SerializeField]
    private bool Q_and_E_to_rotate;


    private Rigidbody2D rb;

    public static Vector2 global_movement_dir = Vector2.zero;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (FollowsMousePos) faceTowardsMouse();
        if (Q_and_E_to_rotate) Rotation();

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

        bool moving = true;

        Vector2 movement = new Vector2(x_movement, y_movement);
        if (movement != Vector2.zero) {
            movement = movement.normalized * movement_speed;
            moving = false;
        }

        rb.velocity = movement;
        global_movement_dir = movement;

        void faceTowardsMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 dir = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            transform.up = dir;
        }
        void Rotation()
        {
            if (Input.GetKey("q"))
            {
                transform.Rotate(new Vector3(0, 0, rotation_speed )* Time.deltaTime);
            }
        
            if (Input.GetKey("e"))
            {
                transform.Rotate(new Vector3(0, 0, -rotation_speed )* Time.deltaTime);
            }
        }
    }
}
