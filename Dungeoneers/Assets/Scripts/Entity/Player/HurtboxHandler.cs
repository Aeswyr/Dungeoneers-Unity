using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtboxHandler : MonoBehaviour
{

    [SerializeField] private UnityEvent action;
    private Collider2D lastCollision;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Owner>().IsOwner(gameObject))
            return;

        lastCollision = other;
        action.Invoke();
    }

    public Collider2D GetLastCollision() {
        return lastCollision;
    }
}
