using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseMob : MonoBehaviour
{

    public enum MobState
    {
        IDLE,
        AGGRESSIVE,
        CHASING,
        EVADING,
        STALKING,
        CASTING,
        DASHING
    }


    public GameObject player;
    private MobState _state = MobState.IDLE;
    public MobState state
    {
        get { return _state; }
        set { _state = value; if (_state != value) Debug.Log("[Mob State] Transfering " + this.name + " from " + _state.ToString() + " to " + value.ToString()); }
    }

    public float viewDist = 20;
    public float fov = 90;
    public float angularSpeed = 1.0f;
    public float speed = 1.0f;

    public float aggression = 10.0f;
    private float sinceLastSeen = 0.0f;

    public int health = 2;

    protected float playerDist;

    public virtual MobState UpdateState()
    {
        bool canSee = false;
        Vector3 d = player.transform.position - transform.position;
        playerDist = d.magnitude;
        d = d.normalized;

        canSee = Vector3.Angle(d, transform.forward) < fov && playerDist < viewDist;

        if (state == MobState.IDLE)
        {
            if (canSee)
                return MobState.AGGRESSIVE;
        }
        else
        {
            if (canSee)
                sinceLastSeen = 0.0f;
            else
                sinceLastSeen += Time.deltaTime;

            if (sinceLastSeen >= aggression)
            {
                sinceLastSeen = 0.0f;
                return MobState.IDLE;
            }
        }

        return state;
    }


    //The Non-Agency Events
    public virtual bool OnDeath()
    {
        //Handle Kill or stage event
        return true;
    }

    public virtual void OnKill()
    {
        //Handle events on the actual kill such as heal.
        Debug.Log("KILL, BLOOD, CARNAGE");
    }

    public virtual void OnReceiveDamage()
    {
        health -= 1;
        if (health == 0)
        {
            if (OnDeath())
            {
                OnKill();
                Destroy(gameObject);
            }
        }
    }


    //Agency Events (Stubs) All should be defined in cased of external effects
    public virtual void OnIdle() { }
    public virtual void OnAggressive() { }
    public virtual void OnChasing() { }
    public virtual void OnEvading() { }
    public virtual void OnStalking() { }
    public virtual void OnCasting() { }
    public virtual void OnDashing() { }



    public void turnTowardsPlayer()
    {
        Vector3 d = player.transform.position - transform.position;
        double angle = Math.Atan2(transform.forward.z - d.z, transform.forward.x - d.x);
        Debug.Log(angle);
        if (angle < 0)
            transform.forward = Quaternion.Euler(0, angularSpeed * Time.deltaTime, 0) * transform.forward;
        else
            transform.forward = Quaternion.Euler(0, -angularSpeed * Time.deltaTime, 0) * transform.forward;
    }
    public void moveTowardsPlayer()
    {
        //Allows moonwalking
        Vector3 d = (player.transform.position - transform.position).normalized;
        transform.position += d * speed * Time.deltaTime;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Debug, to be removed, or replaced with a random rotation
        transform.forward = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        state = UpdateState();

        switch (state)
        {
            case MobState.IDLE:
                OnIdle();
                break;
            case MobState.AGGRESSIVE:
                OnAggressive();
                break;
            case MobState.CHASING:
                OnChasing();
                break;
            case MobState.EVADING:
                OnEvading();
                break;
            case MobState.STALKING:
                OnStalking();
                break;
            case MobState.CASTING:
                OnCasting();
                break;
            case MobState.DASHING:
                OnDashing();
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Sword")
        {
            OnReceiveDamage();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Sword")
        {
            OnReceiveDamage();
        }
    }
}
