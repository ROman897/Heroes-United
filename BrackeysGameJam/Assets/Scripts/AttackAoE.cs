using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAoE : MonoBehaviour
{
    private HashSet<Character> enemies_in_hitbox = new HashSet<Character>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void trigger(Effect effect) {
        foreach (Character character in enemies_in_hitbox) {
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
