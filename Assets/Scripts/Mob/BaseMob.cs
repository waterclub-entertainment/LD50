using UnityEngine;
using UnityEngine.AI;

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
        COOLDOWN,
        DASHING
    }


    protected GameObject player;
    protected MobTarget tgt;
    public GameObject bloodOrbPrefab;
    private MobState _state = MobState.IDLE;
    public MobState state
    {
        get { return _state; }
    }


    public float viewDist = 20;
    public float fov = 90;
    public float angularSpeed = 120f;
    public float speed = 1.0f;

    public float aggression = 5.0f;
    private float sinceLastSeen = 0.0f;

    public HighscoreData scoreObj;
    public int score = 10;

    public int health = 2;
    public NavMeshAgent navMeshAgent;

    protected float playerDist;

    public virtual MobState UpdateState()
    {
        bool canSee = false;
        Vector3 d = player.transform.position - transform.position;
        playerDist = d.magnitude;
        d = d.normalized;

        canSee = Vector3.Angle(d, transform.forward) < fov && playerDist < viewDist;
        canSee |= playerDist < viewDist * 0.5f;

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
        scoreObj.addScore(score);
        
        GameObject res = Instantiate(bloodOrbPrefab, transform.position, Quaternion.identity);
        Debug.Log(res.transform.position);
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
        Vector3 d = (player.transform.position - transform.position).normalized;
        float angle = Vector2.SignedAngle(new Vector2(d.x, d.z), new Vector2(transform.forward.x, transform.forward.z));
        float mult = 1.0f;
        //rotate faster if behind.
        if (Mathf.Abs(angle) > 90)
            mult = 2.0f;
        if (Mathf.Abs(angle) < 5)
            return;
        transform.Rotate(new Vector3(0, angularSpeed * Time.deltaTime * mult * Mathf.Sign(angle), 0));
    }

    public void moveTowardsPlayer()
    {
        navMeshAgent.destination = player.transform.position;
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        tgt = player.GetComponent<MobTarget>(); //Mayhaps make static
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = angularSpeed;
        //Debug, to be removed, or replaced with a random rotation
    }

    // Update is called once per frame
    void Update()
    {
        MobState _s = UpdateState();
        if (state != _s) {
            Debug.Log("[Mob State] Transfering " + this.name + " from " + state.ToString() + " to " + _s.ToString());
            navMeshAgent.ResetPath();
        }
        _state = _s;

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
}
