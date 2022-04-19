using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy, IDamageable
{
    private SpriteRenderer sr;
    public Transform swalowMouth;

    public override void Init()
    {
        base.Init();
        sr = GetComponent<SpriteRenderer>();
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

    public void SwalowBomb() {
        if (targetPoint != null && targetPoint.CompareTag("Bomb")) {

            //targetPoint.GetComponent<Bomb>().TurnOff();
            targetPoint.localScale /= 2;

            targetPoint.position = swalowMouth.position;
            targetPoint.SetParent(swalowMouth);

        }
    }

    public void EatBomb()
    {
        if (targetPoint != null && targetPoint.CompareTag("Bomb"))
        {
            Destroy(targetPoint.gameObject);

        }
    }
}
