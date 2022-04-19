using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Enemy, IDamageable
{
    private SpriteRenderer sr;
    public Transform scareRunPoint;

    public bool isRunAway;
    

    Rigidbody2D rb;

    public override void Init()
    {
        base.Init();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetHit(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 1)
        {
            currentHealth = 0;
            isDead = true;
            sr.sortingOrder = -99;
        }
        anim.SetTrigger("GetHit");
    }

    public void ScareRun() {
        if (targetPoint != null && targetPoint.CompareTag("Bomb")) {

            StartCoroutine(RunAwayBomb(targetPoint.gameObject));
        }
    }

    IEnumerator RunAwayBomb(GameObject bomb) {
        //FlipDireaction(scareRunPoint);
        if (FlipDirc == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        isRunAway = true;
        while (bomb != null) {
            //targetPoint = scareRunPoint;

            //transform.position = Vector2.MoveTowards(transform.position, scareRunPoint.position, speed * Time.deltaTime);
            transform.position += new Vector3(speed/2 * Time.deltaTime * FlipDirc,0,0);
            yield return null;
        }
        isRunAway = false;
        rb.velocity = Vector2.zero;
    }

    public override void MovementToTarget()
    {
        if (isRunAway) return;
        base.MovementToTarget();
    }
}
