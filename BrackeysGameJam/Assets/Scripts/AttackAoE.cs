using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAoE : MonoBehaviour
{
    private HashSet<Character> enemies_in_hitbox = new HashSet<Character>();
    private List<Character> characters_to_remove = new List<Character>();

    public void trigger(Effect effect) {
        List<Character> hit_enemies = new List<Character>();

        foreach (Character character in enemies_in_hitbox) {
            hit_enemies.Add(character);
        }

        foreach (Character character in hit_enemies) {
            character.apply_effect(effect);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        enemies_in_hitbox.Add(other.transform.parent.GetComponent<Character>());
    }

    void OnTriggerExit2D(Collider2D other) {
        enemies_in_hitbox.Remove(other.transform.parent.GetComponent<Character>());
    }

}
