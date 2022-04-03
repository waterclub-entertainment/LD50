using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMob : BaseMob
{
    //defines a range to avoid state flip flopping
    public float comfortableRange = 10.0f;
    //maximum range of dash
    public float dashRange = 15.0f;
    //collision distance
    public float collisionDistance = 2.0f;

    //animation variable
    public float dashSpeed = 1.0f;

    //State machine delegates
    private bool finishedCasting;
    private bool finishedDashing;
    //Dash properties
    private bool hasHit;
    private Vector3 dashStart;

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
            if (playerDist < comfortableRange)
                return MobState.AGGRESSIVE;
        }
        else if (state == MobState.CASTING)
        {
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
        turnTowardsPlayer();
        moveTowardsPlayer();
    }
    public override void OnCasting()
    {
        turnTowardsPlayer();
        if (playerDist < dashRange)
        {
            animator.SetTrigger("Attack");
        }

    }
    public override void OnDashing()
    {
        if ((!hasHit) && playerDist < collisionDistance)
        {
            //not really an animation here, is there?
            hasHit = true;
            tgt.health -= 1;
            animator.SetTrigger("Hit");

            if (tgt.health <= 0)
            {
                //To Be Changed
                Debug.Log("YOU DIED");
            }
        }

        //Computed in Code not in state machine to easier manipulate the distance
        float d = dashRange - (dashStart - transform.position).magnitude;
        if (d <= dashSpeed * Time.deltaTime)
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
        dashStart = transform.position;

        //forward to state machine
        finishedCasting = true;
    }
    //Forward to state machine
    public void OnDashEnd()
    {
        finishedDashing = true;
    }
}
