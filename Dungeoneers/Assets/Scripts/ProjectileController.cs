using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int id;
    [SerializeField] private bool destroyOnContact;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Owner owner;
    [SerializeField] private int vfxId;
    public void Fire(int dir) {
        rbody.velocity = new Vector2(speed * dir, 0);
        sprite.flipX = dir < 0;
        animator.SetInteger("id", id);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (destroyOnContact && !owner.IsOwner(other.gameObject)) {
            if (vfxId != -1)
                GameMaster.Instance.CreateVfx(vfxId).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
