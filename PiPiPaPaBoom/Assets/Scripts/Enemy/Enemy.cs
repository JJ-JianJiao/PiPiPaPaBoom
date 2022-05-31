using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable
{
    EnemyBaseState currentState;

    protected int FlipDirc { get { return transform.rotation.eulerAngles.y == 180 ? -1 : 1; } }

    [Header("Boss")]
    public bool isBoss;

    [Header("Movement")]
    public float speed;

    [Header("Attack Settings")]
    public float attackRate;
    public float attackRange;
    public float skillRange;
    private float nextAttack = 0;
    public bool hasSkill = true;


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

    public bool HitAction { get { return anim.GetCurrentAnimatorStateInfo(1).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(1).IsName("Skill"); } }


    public virtual void Init() {
        anim = GetComponent<Animator>();
        findPlayerSign = transform.GetChild(0).gameObject;
    }

    private void Awake()
    {
        Init();
        currentHealth = fullHealth;
        UIManager.instance.SetBossHealth(currentHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionState(patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        RemoveAttackTargetNull();

        if (isDead) {
            anim.SetBool("Die", isDead);
            return;
        }

        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);
    }

    private void RemoveAttackTargetNull()
    {
        for (int i = 0; i < attackTargetList.Count; i++)
        {
            if (attackTargetList[i] == null) {
                attackTargetList.RemoveAt(i);
            }
        }
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
        if (!hasSkill) return;
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

    public virtual void MovementToTarget() {
        if (targetPoint == null) return;
        if (HitAction) return;
        //if (FlipDirc == 1)
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, new Vector3(targetPoint.position.x - attackRange, targetPoint.position.y, targetPoint.position.z), speed * Time.deltaTime);

        //}
        //else {
        //    transform.position = Vector2.MoveTowards(transform.position, new Vector3(targetPoint.position.x + attackRange, targetPoint.position.y, targetPoint.position.z), speed * Time.deltaTime);
        //}
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        FlipDireaction();
    }

    public void FlipDireaction() {
        if (targetPoint.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            findPlayerSign.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            findPlayerSign.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void FlipDireaction(Transform destination)
    {
        if (destination.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            findPlayerSign.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            findPlayerSign.transform.rotation = Quaternion.Euler(0, 180, 0);
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
                if(hasSkill)
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
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack") || isDead) return;

        if (attackTargetList.Contains(collision.transform))
            attackTargetList.Remove(collision.transform);

        if (attackTargetList.Count == 0 && !(targetPoint == pointA || targetPoint == pointB)) {
            StartCoroutine(OnAlarm("LostTarget_Sign"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack")) return;

        if (!isDead) {

            if( (collision.CompareTag("Player")&&!collision.GetComponent<PilipalaController>().isdead) || collision.CompareTag("Bomb"))
                StartCoroutine(OnAlarm("FindPlayer"));

        }
    }

    IEnumerator OnAlarm(string clip) {
        findPlayerSign.SetActive(true);
        //isStiff = true;
        findPlayerSign.GetComponent<Animator>().Play(clip);
        yield return new WaitForSeconds(1f);
        //isStiff = false;
        findPlayerSign.SetActive(false);
    }

    public virtual void GetHit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 1)
        {
            currentHealth = 0;
            isDead = true;            
        }
        if (isBoss) {
            UIManager.instance?.UpdateBossHealth(currentHealth);
        }
        anim.SetTrigger("GetHit");
    }
}
