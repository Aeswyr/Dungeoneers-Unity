using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottedClaymanController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private SpriteRenderer sprite;
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
        if (dist > 4) {
            rbody.velocity = new Vector2(dir, 0);
        } else {
            rbody.velocity = Vector2.zero;
        }

        if (dir != 0)
            sprite.flipX = dir < 0;


            
    }
}
