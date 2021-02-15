using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : UnityEvent<GameObject> {}

public class CollisionTrigger : MonoBehaviour
{
    public CollisionEvent on_trigger_enter = new CollisionEvent();
    public CollisionEvent on_trigger_exit = new CollisionEvent();

    void OnTriggerEnter2D(Collider2D other) {
        on_trigger_enter.Invoke(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other) {
        on_trigger_exit.Invoke(other.gameObject);
    }
}
