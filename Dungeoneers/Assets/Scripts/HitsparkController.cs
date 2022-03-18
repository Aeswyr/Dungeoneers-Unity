using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitsparkController : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Animator animator;

    void Start()
    {
        animator.SetInteger("id", id);
    }

}
