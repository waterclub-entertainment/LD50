using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMob : BaseMob
{
    public int damagePerHit = 1;
    public float minAttackRange = 2.0f;
    public float maxAttackRange = 3.0f;
    public Animator animator;

    public override MobState UpdateState()
    {
        MobState state = base.UpdateState();

        if (state == MobState.AGGRESSIVE)
        {
            if (playerDist > maxAttackRange)
                return MobState.CHASING;
        }
        else if (state == MobState.CHASING)
        {
            if (playerDist < minAttackRange)
                return MobState.AGGRESSIVE;
        }
        
        return state;
    }

    public override bool OnDeath()
    {
        //Handle Kill or stage event
        //Play Sound
        return base.OnDeath();
    }

    public override void OnKill()
    {
        //Handle events on the actual kill such as heal.
        Debug.Log("UHGGGGG");
        base.OnKill();
    }

    public override void OnReceiveDamage()
    {
        base.OnReceiveDamage();
    }


    public override void OnIdle()
    {
        //maximum half rotation in each direction when idle.
        viewVector = Quaternion.Euler(0, (Random.value - 0.5f) * 90.0f * angularSpeed * Time.deltaTime, 0) * viewVector;
        transform.position += viewVector * speed * Time.deltaTime;
    }
    public override void OnAggressive()
    {
        turnTowardsPlayer();
        if (playerDist < maxAttackRange)
            animator.SetTrigger("Attack");
    }
    public override void OnChasing()
    {
        turnTowardsPlayer();
        moveTowardsPlayer();
    }

    public void OnApplyAttack()
    {
        tgt.Hurt(damagePerHit);
    }
    
}
