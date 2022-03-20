using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float vel;
    [SerializeField] private Rigidbody2D rbody;


    // Update is called once per frame
    void FixedUpdate()
    {
        rbody.velocity = new Vector2(vel, 0);
    }
}
