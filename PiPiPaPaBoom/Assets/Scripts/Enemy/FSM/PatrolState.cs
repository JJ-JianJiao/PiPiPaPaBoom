using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        //Debug.Log(enemy.animState);
        enemy.SwitchPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {


        //if (enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemy.animState = 1;
            //Debug.Log(enemy.animState);

            enemy.MovementToTarget();
        }

        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01)
        {
            enemy.TransitionState(enemy.patrolState);
        }

        if (enemy.attackTargetList.Count > 0) {
            enemy.TransitionState(enemy.attackState);

        }
    }
}
