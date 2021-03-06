using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy, IDamageable
{
    private SpriteRenderer sr;
    public Transform swalowMouth;

    public int swallowBombNum = 0;
    public float increaseRate;

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
    //        sr.sortingOrder = -99;
    //    }
    //    anim.SetTrigger("GetHit");
    //}

    public override void GetHit(int damage)
    {
        base.GetHit(damage);
        if (currentHealth < 1)
        {
            sr.sortingOrder = -98;
        }
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

            AudioManager.Instance?.Play(SoundName.Swallow);

            swallowBombNum++;
            transform.localScale *= (1 + increaseRate * swallowBombNum) ;

            if (swallowBombNum == 5) {
                currentHealth = 0;
                isDead = true;
                GameManager.Instance.RemoveEnemy(this);
                UIManager.instance.UpdateBossHealth(currentHealth);
                sr.sortingOrder = -99;

                

                if (!isPlayDieSound)
                {
                    AudioManager.Instance?.Play(SoundName.WhaleDie);
                    AudioManager.Instance?.Play(SoundName.BoomExplode);
                    isPlayDieSound = true;
                }



            }
        }
    }
}
