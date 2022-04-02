using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMob : BaseMob
{

    public enum AttackState
    {
        IDLE,
        CHARGING,
        ATTACKING,
        COOLDOWN
    }

    public float attackInterval = 1.0f;

    public override MobState UpdateState()
    {
        state = base.UpdateState();

        if (state == MobState.AGGRESSIVE)
        {
            if (playerDist > 1)
                state = MobState.CHASING;
        }
        else if (state == MobState.CHASING)
        {
            if (playerDist < 0.5)
                state = MobState.AGGRESSIVE;
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
        transform.forward = Quaternion.Euler(0, (Random.value - 0.5f) * Time.deltaTime, 0) * transform.forward;
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    public override void OnAggressive()
    {
        turnTowardsPlayer();
        if (playerDist < 1.0f)
            Debug.Log("Attack");
    }
    public override void OnChasing()
    {
        turnTowardsPlayer();
        moveTowardsPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
