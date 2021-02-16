using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffect : MonoBehaviour
{
    [SerializeField]
    private Effect effect;

    void OnTriggerEnter2D(Collider2D other) {
        other.transform.parent.GetComponent<Character>().apply_effect(effect);
        GameObject.Destroy(gameObject);
    }
}
