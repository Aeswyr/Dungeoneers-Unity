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
    [SerializeField] private Collider2D hurtbox;
    [SerializeField] private GameObject spellBrandPrefab;
    [SerializeField] private GameObject attackSlashPrefab;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject starspearPrefab;
    private int facing = 1;

    private bool grounded, running, jumping, cancellable, interruptable, movable = true, singleInput;
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
        singleInput = true;
        running = false;
        jumping = false;
        bool lastgrounded = grounded;
        grounded = ground.CheckGrounded();
        bool landing = grounded & !lastgrounded;

        if (cancellable || interruptable) {
            UpdateFacing();
        }

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
            running = true;
        }

        if (movable &&  interruptable && grounded && singleInput && input.jump.pressed) {
            rbody.velocity = new Vector2(rbody.velocity.x, 0.15f);
            jump.StartJump();
            animator.SetTrigger("jump");
            jumping = true;
            singleInput = false;
        }
        if (cancellable && singleInput && input.primary.pressed) {
            animator.SetTrigger("attack");
            GameObject smear = Instantiate(attackSlashPrefab, transform);
            smear.GetComponent<SpriteRenderer>().flipX = sprite.flipX;
            smear.GetComponent<Animator>().SetInteger("attack_id", attack_id);
            animator.SetInteger("attack_id", attack_id);
            attack_id  = (attack_id + 1) % 2;
            singleInput = false;
        }
        if (grounded && cancellable && singleInput && input.skill1.pressed) {
            animator.SetTrigger("attack");
            animator.SetInteger("attack_id", 2);
            GameObject brand = Instantiate(spellBrandPrefab, transform.position, spellBrandPrefab.transform.rotation);
            brand.GetComponent<Animator>().SetInteger("id", 0);

            GameObject attack = Instantiate(fireballPrefab, transform.position + new Vector3(facing * 0.25f, 0, 0), fireballPrefab.transform.rotation);
            attack.GetComponent<ProjectileController>().Fire(facing);
            attack.GetComponent<Owner>().SetOwner(hurtbox.gameObject);
            singleInput = false;
        }
        if (grounded && cancellable && singleInput && input.skill2.pressed) {
            animator.SetTrigger("attack");
            animator.SetInteger("attack_id", 2);
            GameObject brand = Instantiate(spellBrandPrefab, transform.position, spellBrandPrefab.transform.rotation);
            brand.GetComponent<SpriteRenderer>().flipX = sprite.flipX;
            brand.GetComponent<Animator>().SetInteger("id", 1);

            GameObject attack = Instantiate(starspearPrefab, transform.position + new Vector3(facing * 3, 1.5f, 0), starspearPrefab.transform.rotation);
            attack.GetComponent<ProjectileController>().Fire(facing);
            attack.GetComponent<Owner>().SetOwner(hurtbox.gameObject);
            singleInput = false;
        }
    }

    void LateUpdate() {
        animator.SetBool("running", running);
        animator.SetBool("jumping", jumping);
        animator.SetBool("falling", !grounded);
        animator.SetFloat("x_speed", rbody.velocity.x);
        animator.SetFloat("y_speed", rbody.velocity.y);
    }

    void UpdateFacing() {
        if (input.dir != 0) {
            sprite.flipX = input.dir < 0;
            facing = (int)input.dir;
        }
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

    private void AttackMovement(float amount) {
        if (input.dir == 0)
            return;
        rbody.velocity = new Vector2(amount * facing, rbody.velocity.y);
        move.StartDeceleration(facing);
    }

    public void LinkInputs(InputHandler input) {
        this.input = input;
        jump.LinkInputs(input);
    }
}
