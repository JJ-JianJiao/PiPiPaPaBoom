using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy, IDamageable
{
    private SpriteRenderer sr;

    public override void Init()
    {
        base.Init();
        sr = GetComponent<SpriteRenderer>();
    }


    public void GetHit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 1) {
            currentHealth = 0;
            isDead = true;
            sr.sortingOrder = -99;
        }
        anim.SetTrigger("GetHit");
    }

    public void SetBombOff() { //animation event
        if(targetPoint != null)
            if(targetPoint.CompareTag("Bomb"))
                targetPoint.GetComponent<Bomb>().TurnOff();
    }
}
