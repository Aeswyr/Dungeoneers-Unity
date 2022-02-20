using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rbody;
    private InputHandler input;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private JumpHandler jump;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private float speed;

    private bool grounded, running, jumping, cancellable, interruptable;
    private int attack_id = -1;

    // Start is called before the first frame update
    void Start()
    {
        SetInterruptable();
        SetCancellable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        running = false;
        jumping = false;
        bool lastgrounded = grounded;
        grounded = ground.CheckGrounded();
        bool landing = grounded & !lastgrounded;

        if (landing) {
            jump.ForceLanding();
            animator.SetTrigger("land");
        }
            

        if (interruptable && input.dir != 0) {
            rbody.velocity = new Vector2(speed * input.dir, rbody.velocity.y);
            sprite.flipX = input.dir < 0;
            running = true;
        }
        if (interruptable && grounded && input.jump.pressed) {
            rbody.velocity = new Vector2(rbody.velocity.x, 0.15f);
            jump.StartJump();
            animator.SetTrigger("jump");
            jumping = true;
        }
        if (grounded && cancellable && input.primary.pressed) {
            animator.SetTrigger("attack");
            if (attack_id == 0) {
                attack_id = 1;
            } else {
                attack_id = 0;
            }
            animator.SetInteger("attack_id", attack_id);
        }
    }

    void LateUpdate() {
        animator.SetBool("running", running);
        animator.SetBool("jumping", jumping);
        animator.SetBool("falling", !grounded);
        animator.SetFloat("x_speed", rbody.velocity.x);
        animator.SetFloat("y_speed", rbody.velocity.y);
    }

    private void SetCancellable() {
        cancellable = true;
        animator.SetBool("cancellable", cancellable);
    }

    private void UnsetCancellable() {
        cancellable = false;
        animator.SetBool("cancellable", cancellable);
    }

    private void SetInterruptable() {
        interruptable = true;
        animator.SetBool("interruptable", interruptable);
    }

    private void UnsetInterruptable() {
        interruptable = false;
        animator.SetBool("interruptable", interruptable);
    }

    private void ToAttack() {
        UnsetInterruptable();
        UnsetCancellable();
    }

    private void FromAttack() {
        SetInterruptable();
        SetCancellable();
        attack_id = -1;
    }

    public void LinkInputs(InputHandler input) {
        this.input = input;
        jump.LinkInputs(input);
    }
}
