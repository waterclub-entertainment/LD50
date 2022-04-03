using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMob : AttackMob
{
    //Need to wait for animation before transferring out.
    public GameObject projectilePrefab;
    private bool hasCasted = false;

    public override MobState UpdateState()
    {
        MobState state = base.UpdateState();

        if (state == MobState.AGGRESSIVE)
        {
            if (playerDist > maxAttackRange)
                return MobState.CHASING;
            else if (playerDist < minAttackRange)
                return MobState.EVADING;
            else
            {
                hasCasted = false;
                return MobState.CASTING;
            }
        }
        else if (state == MobState.CHASING)
        {
            if (playerDist < maxAttackRange)
                return MobState.AGGRESSIVE;
        }
        else if (state == MobState.CASTING && hasCasted)
        {
            return MobState.AGGRESSIVE;
        }
        else if (state == MobState.EVADING)
        {
            if (playerDist > minAttackRange)
                return MobState.AGGRESSIVE;
        }

        return state;
    }

    public override void OnCasting()
    {
        turnTowardsPlayer();
        if (playerDist < maxAttackRange)
            animator.SetTrigger("Attack");
    }
    //When not shooting really not do anything i guess
    public override void OnAggressive()
    {
        turnTowardsPlayer();
    }
    public override void OnEvading()
    {
        if (navMeshAgent.remainingDistance < 0.2) {
            Vector2 r = Random.insideUnitCircle;
            navMeshAgent.destination = transform.position + (transform.position - player.transform.position).normalized * 2f;
        }
    }

    //Animation Triggers
    new public void OnApplyAttack()
    {
        //spawn projectile in front of own position.
        Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
    }
    public void OnFinishedAttack()
    {
        hasCasted = true;
    }
}
