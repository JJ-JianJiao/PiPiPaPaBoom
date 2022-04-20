using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy, IDamageable
{
    private SpriteRenderer sr;
    public Transform pickupPoint;
    public float throwBombForce;
    public Vector2 throwBormDirection;

    public override void Init()
    {
        base.Init();
        sr = GetComponent<SpriteRenderer>();
    }

    public void GetHit(int damage)
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

    public void PickUpBomb() //animation Event
    {
        if (targetPoint != null && targetPoint.CompareTag("Bomb")){
            
            targetPoint.transform.SetParent(pickupPoint);
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            targetPoint.transform.localPosition = Vector3.zero;
        }
    }

    public void ThrowBomb() {
        if (pickupPoint.childCount!=0 && pickupPoint.GetChild(0).CompareTag("Bomb"))
        {

            if (targetPoint == null || !targetPoint.CompareTag("Bomb")) return;
            targetPoint.transform.SetParent(null);
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            throwBormDirection.Normalize();
            targetPoint.GetComponent<Rigidbody2D>().velocity = new Vector2(FlipDirc * throwBormDirection.x, throwBormDirection.y) * throwBombForce;
        }
    }
}
