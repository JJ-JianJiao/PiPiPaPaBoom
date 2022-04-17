using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{

    public float damage;
    public float addForce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player get hurt");
            other.GetComponent<IDamageable>().GetHit(damage);
        }
        else if (other.CompareTag("Bomb")) {
            Debug.Log("Skill on Bomb");
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * 2, 1f) * addForce, ForceMode2D.Impulse);
        }
    }
}
