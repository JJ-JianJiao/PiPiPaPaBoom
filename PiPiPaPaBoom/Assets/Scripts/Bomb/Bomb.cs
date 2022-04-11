using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator anim;
    public float startTime;
    public float waitTime;
    Collider2D coll;
    Rigidbody2D rb;
    [Header("Check")]
    public float explotionRadius;
    public LayerMask targetLayer;
    public float explotionForce;


    public float damage;
    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    private void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BombOff"))
        {

            if (Time.time > startTime + waitTime)
            {
                anim.Play("BombExplotion");

            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explotionRadius);
    }


    //Animation Events
    public void Explotion() {
        coll.enabled = false;
        Collider2D[] aroundObjs = Physics2D.OverlapCircleAll(transform.position, explotionRadius, targetLayer);
        rb.gravityScale = 0;
        foreach (var item in aroundObjs)
        {
            Vector3 itemPos = transform.position - item.transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((-itemPos+Vector3.up) * explotionForce, ForceMode2D.Impulse);

            if (item.CompareTag("Bomb") && item.GetComponent<Bomb>().anim.GetCurrentAnimatorStateInfo(0).IsName("BombOff")) {
                item.GetComponent<Bomb>().TurnOn();
            }
            if (item.CompareTag("Player")) {
                item.GetComponent<IDamageable>().GetHit(damage);
            }
            if (item.CompareTag("Enemy")) {
                item.GetComponent<IDamageable>().GetHit(damage);
            }
        }

    }

    public void DestroyThis() {
        Destroy(this.gameObject);
    }

    public void TurnOff() {
        anim.Play("BombOff");
        gameObject.layer = LayerMask.NameToLayer("NoFireBomb");
    }

    public void TurnOn() {
        startTime = Time.time;
        anim.Play("BombOn");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }
}
