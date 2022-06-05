using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public int currentHealth;
    public int fullHealth;
    public bool isdead;
    public bool Invincible { get { return anim.GetCurrentAnimatorStateInfo(1).IsName("Pilipala_GetHurt");} }

    private PilipalaInputSys controls;
    Vector2 move = new Vector2();
    bool triggerAttack;



    private void Awake()
    {
        controls = new PilipalaInputSys();
        controls.Player.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => move = Vector2.zero;
        controls.Player.Jump.started += ctx => allowJump = isGround?true:false;
        controls.Player.DropBomb.started += ctx => triggerAttack = true;
        controls.Player.DropBomb.canceled += ctx => triggerAttack = false;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = fullHealth;
        GameManager.Instance.playerController = this;
        currentHealth = GameManager.Instance.LoadPlayerData();
        UIManager.instance.UpdatePlayerHealth(currentHealth);
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);
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
        if (Invincible) return;

        Movement();
        Jump();
    }

    void Movement() {

        //float horizontalInput = Input.GetAxisRaw("Horizontal");
        //rb.velocity = new Vector2(speed * horizontalInput, rb.velocity.y);
        //Flip(horizontalInput);

        rb.velocity = new Vector2(speed * move.x, rb.velocity.y);
        Flip(move.x);
    }

    void Flip(float horizontalInput) {
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput > 0 ? 1 : -1 , transform.localScale.y, transform.localScale.z);
        }
    }

    void CheckInput() {
        //if (Input.GetButtonDown("Jump") && isGround)
        //{
        //    allowJump = true;
        //}
        //if (Input.GetKeyDown(KeyCode.K)) {
        //    Attack();
        //}

        if (triggerAttack) {
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

    public void GetHit(int damage)
    {
        if (Invincible)
            return;
        currentHealth -= damage;
        if (currentHealth < 1) {
            currentHealth = 0;
            isdead = true;
            GameManager.Instance.GameOver();
        }
        anim.SetTrigger("Hit");
        UIManager.instance.UpdatePlayerHealth(currentHealth);
    }
}
