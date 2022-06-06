using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{

    public int damage;
    public float addForce;
    public float pushPlayerForce;
    public Vector2 forceDirection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player get hurt");
            other.GetComponent<IDamageable>().GetHit(damage);
            if (pushPlayerForce == 0) return;
            int kickDirection = this.transform.parent.localEulerAngles.y == 180 ? -1 : 1;
            forceDirection.Normalize();
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(forceDirection.x * kickDirection, forceDirection.y) * pushPlayerForce;
            //other.GetComponent<Rigidbody2D>().AddForce(forceDirection * kickDirection * pushPlayerForce, ForceMode2D.Impulse);
        }
        else if (other.CompareTag("Bomb")) {
            Debug.Log("Skill on Bomb");

            

            transform.parent.GetComponent<Enemy>().attackTargetList.Remove(other.transform);
            int kickDirection = this.transform.parent.localEulerAngles.y == 180?-1:1;
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(kickDirection * 2, 1f) * addForce, ForceMode2D.Impulse);

            if (transform.parent.parent.name.Contains("Bald")) {
                AudioManager.Instance?.Play(SoundName.KickBall);
            }
        }
    }
}
