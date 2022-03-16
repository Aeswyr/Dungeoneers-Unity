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
    [SerializeField] private MovementHandler move;
    [SerializeField] private GameObject spellBrandPrefab;
    [SerializeField] private GameObject attackSlashPrefab;

    private bool grounded, running, jumping, cancellable, interruptable, movable = true;
    private int attack_id = 0;

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
        
        if (movable && input.move.released)
            move.StartDeceleration();
        if (input.move.pressed)
            move.StartAcceleration(input.dir);
        if (interruptable && input.dir != 0) {
            move.UpdateMovement(input.dir);
            sprite.flipX = input.dir < 0;
            running = true;
        }

        if (movable && interruptable && grounded && input.jump.pressed) {
            rbody.velocity = new Vector2(rbody.velocity.x, 0.15f);
            jump.StartJump();
            animator.SetTrigger("jump");
            jumping = true;
        }
        if (cancellable && input.primary.pressed) {
            animator.SetTrigger("attack");
            GameObject smear = Instantiate(attackSlashPrefab, transform);
            smear.GetComponent<SpriteRenderer>().flipX = sprite.flipX;
            smear.GetComponent<Animator>().SetInteger("attack_id", attack_id);
            animator.SetInteger("attack_id", attack_id);
            attack_id  = (attack_id + 1) % 2;
        }
        if (grounded && cancellable && input.skill1.pressed) {
            animator.SetTrigger("attack");
            animator.SetInteger("attack_id", 2);
            Instantiate(spellBrandPrefab, transform.position, spellBrandPrefab.transform.rotation);
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
        LockMovement();
    }

    private void FromAttack() {
        SetInterruptable();
        SetCancellable();
        UnlockMovement();
    }

    private void LockMovement() {
        movable = false;
        move.StartDeceleration();
    }

    private void UnlockMovement() {
        movable = true;
    }

    public void LinkInputs(InputHandler input) {
        this.input = input;
        jump.LinkInputs(input);
    }
}
