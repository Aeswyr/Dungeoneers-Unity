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
    [SerializeField] private HurtboxHandler hurtboxHandler;
    [SerializeField] private GameObject spellBrandPrefab;
    [SerializeField] private GameObject attackSlashPrefab;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject starspearPrefab;
    private int facing = 1;

    private bool grounded, running, jumping, cancellable, interruptable, movable = true, countering, canCounter, lastDown;
    private bool immune = false;
    private int attack_id = 0;

    private float down = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetInterruptable();
        SetCancellable();
        GameMaster.Instance.RegisterPlayer(this);
        transform.position = new Vector3(-15, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        running = false;
        jumping = false;
        bool lastgrounded = grounded;
        grounded = ground.CheckGrounded();
        bool landing = grounded & !lastgrounded;

        if (cancellable || interruptable) {
            UpdateFacing();
        }

        if ((landing && Time.time < down) || lastDown && Time.time >= down)
            move.Zero();
        if (lastDown && Time.time >= down)
            immune = false;
        animator.SetBool("down", Time.time < down);
        

        if (landing) {
            jump.ForceLanding();
            animator.SetTrigger("land");
        }
        
        if (movable && input.move.released)
            move.StartDeceleration();
        if (movable && input.move.pressed)
            move.StartAcceleration(input.dir);
        if (interruptable && input.dir != 0) {
            move.UpdateMovement(input.dir);
            running = true;
        }

        if (canCounter && (input.primary.pressed || input.skill1.pressed)) {
            animator.SetTrigger("attack");
            animator.SetInteger("attack_id", 5);
            GameObject smear = Instantiate(attackSlashPrefab, transform);
            smear.GetComponent<PrimaryAttack>().Fire(facing, 2, this);
            smear.GetComponent<Owner>().SetOwner(hurtbox.gameObject);
            smear.GetComponent<DestroyAfterDelay>().SetLifetime(5f/12);
            canCounter = false;
            countering = false;
        }
        else if (movable &&  interruptable && grounded && input.jump.pressed) {
            rbody.velocity = new Vector2(rbody.velocity.x, 0.15f);
            jump.StartJump();
            animator.SetTrigger("jump");
            jumping = true;

        }
        else if (cancellable && input.primary.pressed) {
            animator.SetTrigger("attack");
            GameObject smear = Instantiate(attackSlashPrefab, transform);
            smear.GetComponent<PrimaryAttack>().Fire(facing, attack_id, this);
            smear.GetComponent<Owner>().SetOwner(hurtbox.gameObject);
            animator.SetInteger("attack_id", attack_id);
            attack_id  = (attack_id + 1) % 2;
        }
        else if (cancellable && input.skill1.pressed) {
            if (false) {
                animator.SetTrigger("attack");
                animator.SetInteger("attack_id", 3);
            } else {
                animator.SetTrigger("attack");
                animator.SetInteger("attack_id", 4);
            }

        }
        else if (grounded && cancellable && input.skill2.pressed) {
            animator.SetTrigger("attack");
            animator.SetInteger("attack_id", 2);
            CreateBrand(0);

            GameObject attack = Instantiate(fireballPrefab, transform.position + new Vector3(facing * 0.25f, 0, 0), fireballPrefab.transform.rotation);
            attack.GetComponent<ProjectileController>().Fire(facing);
            attack.GetComponent<Owner>().SetOwner(hurtbox.gameObject);
        }
        else if (grounded && cancellable && input.skill3.pressed) {
            animator.SetTrigger("attack");
            animator.SetInteger("attack_id", 2);
            CreateBrand(1);

            GameObject attack = Instantiate(starspearPrefab, transform.position + new Vector3(facing * 3, 1.5f, 0), starspearPrefab.transform.rotation);
            attack.GetComponent<ProjectileController>().Fire(facing);
            attack.GetComponent<Owner>().SetOwner(hurtbox.gameObject);
        }
        lastDown = Time.time < down;
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
        canCounter = false;
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

    private void SkillMovement(float amount) {
        rbody.velocity = new Vector2(amount * facing, rbody.velocity.y);
        move.StartDeceleration(facing);
    }

    public void LinkInputs(InputHandler input) {
        this.input = input;
        jump.LinkInputs(input);
    }

    private void CreateBrand(int id) {
        GameObject brand = Instantiate(spellBrandPrefab, transform.position, spellBrandPrefab.transform.rotation);
        brand.GetComponent<SpriteRenderer>().flipX = sprite.flipX;
        brand.GetComponent<Animator>().SetInteger("id", id);
    }

    public void DoKnockback(Vector2 knockback, float hitpause, bool dampened = true) {
        StartCoroutine(Hitpause(hitpause));
        StartCoroutine(Knockback(knockback, hitpause, dampened));
    }

    IEnumerator Hitpause(float duration) {
        animator.speed = 0;
        yield return new WaitForSeconds(duration);
        animator.speed = 1;
    }
    IEnumerator Knockback(Vector2 knockback, float delay, bool dampened) {
        move.Zero();
        yield return new WaitForSeconds(delay);
        move.ApplyKnockback(knockback, (int)(knockback.x / Mathf.Abs(knockback.x)), dampened);
    }

    public void OnHit() {
        int dir = (int)Mathf.Sign(hurtboxHandler.transform.position.x - hurtboxHandler.GetLastCollision().transform.position.x);
        if (countering) {
            canCounter = true;
            SetCancellable();
            DoKnockback(new Vector2(dir * 10, 5), 0.05f);
        } else if (!immune) {
            down = Time.time + 1f;
            animator.SetTrigger("hurt");
            animator.SetBool("down", Time.time < down);
            DoKnockback(new Vector2(dir * 30, 20), 0.2f, false);
            immune = true;
        }
    }

    private void SetCounter() {
        countering = true;
    }

    private void UnsetCounter() {
        countering = false;
    }
}
