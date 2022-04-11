using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    [Header("Movement")]
    public float speed;

    [Header("Attack Settings")]
    public float attackRate;
    public float attackRange;
    public float skillRange;
    private float nextAttack = 0;


    public Transform pointA, pointB;    //guard pointA and pointB
    public Transform targetPoint;

    public List<Transform> attackTargetList = new List<Transform>();

    public Animator anim;
    public int animState;

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    [Header("Health State")]
    public float currentHealth;
    public float fullHealth;
    public bool isDead;

    private GameObject findPlayerSign;

    public virtual void Init() {
        anim = GetComponent<Animator>();
        findPlayerSign = transform.GetChild(0).gameObject;
    }

    private void Awake()
    {
        Init();
        currentHealth = fullHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionState(patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            anim.SetBool("Die", isDead);
            return;
        }

        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);
    }

    public void TransitionState(EnemyBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }

    public void BasicAttack() {    //attack player
        if (Vector2.Distance(transform.position, targetPoint.position) <= attackRange) {
            if (Time.time > nextAttack) {
                nextAttack = Time.time + attackRate;
                //play attack animation
                Debug.Log("basic attack");

                anim.SetTrigger("attack");
            }
        }
    }

    public virtual void SkillAttack() { //interact with the bomb
        if (Vector2.Distance(transform.position, targetPoint.position) <= skillRange)
        {
            if (Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                //play attack animation
                Debug.Log("Skill attack");
                anim.SetTrigger("skill");
            }
        }
    }

    public void MovementToTarget() {

        if (targetPoint == null) return;
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDireaction();
    }

    public void FlipDireaction() {
        if (targetPoint.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }


    public void SwitchPoint() {
        if (Mathf.Abs(transform.position.x - pointA.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
            targetPoint = pointB;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackTargetList.Contains(collision.transform)) {
            if (collision.CompareTag("Player") )
            {
                if(!collision.GetComponent<PilipalaController>().isdead)
                    attackTargetList.Add(collision.transform);
                return;
            }
            else {
                attackTargetList.Add(collision.transform);
                return;
            }
        }

        if (collision.CompareTag("Player") && attackTargetList.Contains(collision.transform)) {
            if (collision.GetComponent<PilipalaController>().isdead) {
                attackTargetList.Remove(collision.transform);
                return;
            }
        }
       
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (attackTargetList.Contains(collision.transform))
            attackTargetList.Remove(collision.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(OnAlarm());
    }

    IEnumerator OnAlarm() {
        findPlayerSign.SetActive(true);
        yield return new WaitForSeconds(findPlayerSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        findPlayerSign.SetActive(false);
    }
}
