using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    [Header("Movement")]
    public float speed;

    [Header("Attack Settings")]
    private float nextAttack = 0;
    private float attackRate;
    private float attackRange;
    private float skillRange;

    public Transform pointA, pointB;    //guard pointA and pointB
    public Transform targetPoint;

    public List<Transform> attackTargetList = new List<Transform>();

    public Animator anim;
    public int animState;

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init() {
        anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionState(patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);
    }

    public void TransitionState(EnemyBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }

    public void BasicAttack() {    //attack player
        Debug.Log("basic attack");
    }

    public virtual void SkillAttack() { //interact with the bomb
        Debug.Log("Skill attack");
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
        if(!attackTargetList.Contains(collision.transform))
            attackTargetList.Add(collision.transform);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (attackTargetList.Contains(collision.transform))
            attackTargetList.Remove(collision.transform);
    }
}
