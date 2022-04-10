using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilipalaAnimation : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    PilipalaController pilipalaController;
    int animRunID;
    int animJumpID;
    int animGroundID;
    int animFallID;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pilipalaController = GetComponent<PilipalaController>();
        animRunID = Animator.StringToHash("Speed");
        animJumpID = Animator.StringToHash("Jump");
        animGroundID = Animator.StringToHash("Ground");
        animFallID = Animator.StringToHash("VelocityY");
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger(animRunID, (int) Mathf.Abs(rb.velocity.x));
        anim.SetBool(animJumpID, pilipalaController.isJump);
        anim.SetFloat(animFallID, rb.velocity.y);
        anim.SetBool(animGroundID, pilipalaController.isGround);
    }
}
