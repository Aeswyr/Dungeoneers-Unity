using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Owner owner;
    [SerializeField] private Collider2D col;
    private PlayerHandler player;
    int dir;
    public void Fire(int dir, int id, PlayerHandler player) {
        animator.SetInteger("attack_id", id);
        transform.localScale = new Vector3(dir, transform.localScale.y, transform.localScale.z);
        this.dir = dir;
        this.player = player;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (owner.IsOwner(other.gameObject))
            return;
        float force = 10;
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask(new string[] {"World", "Hurtbox"}));
        RaycastHit2D[] cols = new RaycastHit2D[10];
        col.Cast(dir * Vector2.right, filter,  cols, 0, true);

        Vector2 point = Vector2.zero;
        foreach (var contact in cols) {
            if (!owner.IsOwner(contact.collider.gameObject)) {
                point =  contact.point;
                break;
            }
        }
        GameMaster.Instance.CreateVfx(0).transform.position = point;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("World")) {
            force = 20;
            float animtime = animator.GetCurrentAnimatorClipInfo(0).Length * animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animtime >= 0.25/3)
                Destroy(gameObject);
            else
                StartCoroutine(WaitForDestroy(animtime));
        }
        player.DoKnockback(new Vector2(dir * -force, 0), 0.05f);
    }

    private IEnumerator WaitForDestroy(float wait) {
        yield return new WaitForSeconds(0.25f/3 - wait);

        Destroy(gameObject);
    }
    
}
