using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private Character character;

    bool delayed_command = false;
    private Vector2 target_pos;

    void Awake() {
        character = GetComponent<Character>();
    }

    public void move_command(Vector2 world_pos) {
        if (!character.is_alive()) {
            return;
        }
        if (character.get_state() == CharacterState.ATTACKING) {
            delayed_command = true;
            return;
        }
        delayed_command = false;
        character.move_to(world_pos);
    }

    void Update() {
        if (delayed_command) {
            move_command(target_pos);
        }
    }
}
