using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampline : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.Play("Trampline");
    }
}
