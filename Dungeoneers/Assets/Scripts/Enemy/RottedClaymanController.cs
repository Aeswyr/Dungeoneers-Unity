using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottedClaymanController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hurtbox;
    bool attacking = false;
    int facing;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dir = 1;
        float dist = float.MaxValue;
        foreach (var player in GameMaster.Instance.GetPlayers()) {
            if (Mathf.Abs(Vector3.Distance(transform.position, player.transform.position)) < dist) {
                dist = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
                dir = Mathf.Sign(player.transform.position.x - transform.position.x);
            }
        }

        if (dir != 0 && !attacking) {
            facing = (int)dir;
            sprite.flipX = dir < 0;  
        } 

        if (dist > 4 && !attacking) {
            rbody.velocity = new Vector2(dir, 0);
        } else {
            rbody.velocity = Vector2.zero;
            if (!attacking) {
                attacking = true;
                animator.SetTrigger("quickattack");
            }
        }
    }

    private void ResetAttacking() {
        attacking = false;
    }

    private void AttackPhase1() {
        GameObject hitbox = GameMaster.Instance.CreateHitbox(6, 6, hurtbox, 1f/6);
        hitbox.transform.position = transform.position + new Vector3(facing * 3, 0, 0);
    }

    private void AttackPhase2() {
        GameObject hitbox = GameMaster.Instance.CreateHitbox(6, 6, hurtbox);
        hitbox.transform.position = transform.position + new Vector3(facing * -3, 0, 0);
    }
 }
