using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        //Debug.Log("Detect enemy");
        enemy.targetPoint = enemy.attackTargetList[0];
        enemy.animState = 2;


    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.attackTargetList.Count == 0)
        {
            enemy.TransitionState(enemy.patrolState);
        }

        if (enemy.attackTargetList.Count > 1)
        {
            for (int i = 0; i < enemy.attackTargetList.Count; i++)
            {

                if (enemy.attackTargetList[i] != null &&
                    enemy.targetPoint != null &&
                    Mathf.Abs(enemy.attackTargetList[i].transform.position.x - enemy.transform.position.x)
                    < Mathf.Abs(enemy.targetPoint.position.x - enemy.attackTargetList[i].transform.position.x))
                {
                    enemy.targetPoint = enemy.attackTargetList[i];

                }
            }
        }
        if(enemy.attackTargetList.Count == 1) enemy.targetPoint = enemy.attackTargetList[0];



        if (enemy.targetPoint == null) return;

        if (enemy.targetPoint.CompareTag("Player"))
        {

            enemy.BasicAttack();
        }
        else if(enemy.targetPoint.CompareTag("Bomb")) {
            enemy.SkillAttack();
        }

        enemy.MovementToTarget();
    }

}
