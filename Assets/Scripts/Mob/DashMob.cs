using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMob : BaseMob
{
    //defines a range to avoid state flip flopping
    public float comfortableRange = 10.0f;
    //maximum range of dash
    public float dashTime = 2.0f;
    //collision distance
    public float collisionDistance = 2.0f;

    //animation variable
    public float dashSpeed = 1.0f;

    //State machine delegates
    private bool finishedCasting;
    private bool finishedDashing;
    //Dash properties
    private bool hasHit;
    private float dashTimeLeft = 0.0f;
    public int damagePerHit = 1;

    public Animator animator;

    public override MobState UpdateState()
    {
        MobState state = base.UpdateState();

        if (state == MobState.AGGRESSIVE)
        {
            if (playerDist > dashRange)
                return MobState.CHASING;
            else
            {
                finishedCasting = false;
                return MobState.CASTING; //commence charge up
            }
        }
        else if (state == MobState.CHASING)
        {
            animator.SetTrigger("Abort");
            if (playerDist < comfortableRange)
                return MobState.AGGRESSIVE;
        }
        else if (state == MobState.CASTING)
        {
            animator.ResetTrigger("Abort");
            //This transfer is delegate to the animation event. this is just so the state transitions are in one function.
            if (finishedCasting)
            {
                finishedDashing = false;
                return MobState.DASHING;
            }
            //should it cancel once you move out of range?
            else if (playerDist > dashRange)
            {
                animator.SetTrigger("Abort");
                return MobState.AGGRESSIVE;
            }
        }
        else if (state == MobState.DASHING)
        {
            animator.ResetTrigger("Arrived");
            //This transfer is delegate to the animation event. this is just so the state transitions are in one function.
            if (finishedDashing)
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
    
    public override void OnChasing()
    {
        animator.SetTrigger("Abort");
        turnTowardsPlayer();
        moveTowardsPlayer();
    }
    public override void OnCasting()
    {
        finishedCasting = false;
        turnTowardsPlayer();
        if (playerDist < dashRange)
        {
            animator.SetTrigger("Attack");
        }

    }
    public override void OnDashing()
    {
        finishedDashing = false;
        if ((!hasHit) && playerDist < collisionDistance)
        {
            hasHit = true;
            animator.SetTrigger("Arrived");

            tgt.Hurt(damagePerHit);
        }

        //Computed in Code not in state machine to easier manipulate the distance
        float d = dashTimeLeft - Time.deltaTime;
        if (d >= 0.0f)
        {
            //arrive at point
            transform.position += viewVector * d;
            animator.SetTrigger("Arrived");
        }
        else
        {
            transform.position += viewVector * dashSpeed * Time.deltaTime;
        }
    }


    //When not shooting really not do anything i guess
    public override void OnAggressive()
    {
        Vector3 d = player.transform.position - transform.position;
        Vector3 side = Vector3.Cross(Vector3.up, viewVector).normalized; //strave and try to move towards player

        if (Vector3.Angle(d, side) < Vector3.Angle(d, -side))
            transform.position += side * speed * Time.deltaTime;
        else
            transform.position -= side * speed * Time.deltaTime;


        turnTowardsPlayer();
    }

    public void OnDashStart()
    {
        //setup dash and collision state
        hasHit = false;
        dashTimeLeft = dashTime;

        //forward to state machine
        finishedCasting = true;
    }
    //Forward to state machine
    public void OnDashEnd()
    {
        finishedDashing = true;
    }
}
