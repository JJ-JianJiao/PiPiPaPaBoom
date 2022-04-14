using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilipalaController : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;
    public float speed;
    public float jumpForce;



    [Header("States Check")]
    public bool isGround;
    public bool isJump;
    public bool allowJump;

    [Header("Check Points")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("Fx")]
    public GameObject jumpFx;
    private Vector3 jumpFxPositionOffset =  new Vector3(0,-0.472f,0f);
    public GameObject fallFx;
    private Vector3 fallFxPositionOffset = new Vector3(0, -0.746f, 0f);

    [Header("Attack")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;

    [Header("Player State")]
    public float currentHealth;
    public float fullHealth;
    public bool isdead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = fullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isdead)
        {
            anim.SetBool("Dead", isdead);
            return;
        }

        CheckInput();
        //anim.SetBool("Dead", isdead);
    }

    private void FixedUpdate()
    {
        if (isdead) {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsChek();

        Movement();
        Jump();
    }

    void Movement() {

        float horizontalInput = Input.GetAxisRaw("Horizontal"); 

        rb.velocity = new Vector2(speed * horizontalInput, rb.velocity.y);

        Flip(horizontalInput);

    }

    void Flip(float horizontalInput) {
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput, transform.localScale.y, transform.localScale.z);
        }
    }

    void CheckInput() {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            allowJump = true;
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            Attack();
        }
    }

    void Jump() {
        if (allowJump ) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            allowJump = false;
            isJump = true;
            jumpFx.SetActive(true);
            jumpFx.transform.position = this.transform.position + jumpFxPositionOffset;
        }
    }

    public void Attack() {
        if (Time.time > nextAttack) {

            var bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(0, 2) == 1 ? 10 : -10;
            nextAttack = Time.time + attackRate;
        }

    }

    void PhysicsChek() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false;

        }
        else
            rb.gravityScale = 5;
    }

    public void LandFx() {
        fallFx.SetActive(true);
        fallFx.transform.position = transform.position + fallFxPositionOffset;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void GetHit(float damage)
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Pilipala_GetHurt"))
            return;
        currentHealth -= damage;
        if (currentHealth < 1) {
            currentHealth = 0;
            isdead = true;
        }
        anim.SetTrigger("Hit");
    }
}