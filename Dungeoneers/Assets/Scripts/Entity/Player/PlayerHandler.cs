using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private InputHandler input;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private JumpHandler jump;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private float speed;

    private bool grounded, running, jumping;

    // Start is called before the first frame update
    void Start()
    {
        
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
            

        if (input.dir != 0) {
            rbody.velocity = new Vector2(speed * input.dir, rbody.velocity.y);
            sprite.flipX = input.dir < 0;
            running = true;
        }
        if (grounded && input.jump.pressed) {
            rbody.velocity = new Vector2(rbody.velocity.x, 0.15f);
            jump.StartJump();
            animator.SetTrigger("jump");
            jumping = true;
        }


    }

    void LateUpdate() {
        animator.SetBool("running", running);
        animator.SetBool("jumping", jumping);
        animator.SetBool("falling", !grounded);
        animator.SetFloat("x_speed", rbody.velocity.x);
        animator.SetFloat("y_speed", rbody.velocity.y);
    }
}
