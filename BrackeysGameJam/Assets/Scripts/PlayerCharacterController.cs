using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        
    }

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool moving = PlayerController.global_movement_dir != Vector2.zero;
        animator.SetBool("moving", moving);
        if (moving) {
            animator.SetFloat("x_dir", PlayerController.global_movement_dir.x);
            animator.SetFloat("y_dir", PlayerController.global_movement_dir.y);
        }
    }
}
