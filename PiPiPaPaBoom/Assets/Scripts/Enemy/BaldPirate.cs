using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPirate : Enemy
{
    private SpriteRenderer sr;

    public override void Init()
    {
        base.Init();
        sr = GetComponent<SpriteRenderer>();
    }

    //public void GetHit(int damage)
    //{
    //    currentHealth -= damage;
    //    if (currentHealth < 1)
    //    {
    //        currentHealth = 0;
    //        isDead = true;
    //        sr.sortingOrder = -98;
    //    }
    //    anim.SetTrigger("GetHit");
    //}

    public override void GetHit(int damage)
    {
        base.GetHit(damage);
        if (currentHealth < 1) {
            sr.sortingOrder = -98;
        }
    }


}
