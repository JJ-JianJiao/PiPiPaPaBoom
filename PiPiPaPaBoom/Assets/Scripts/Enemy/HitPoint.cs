using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player get hurt");
            other.GetComponent<IDamageable>().GetHit(8);
        }
        else if (other.CompareTag("Bomb")) {

        }
    }
}
