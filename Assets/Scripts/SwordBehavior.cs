using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordBehavior : MonoBehaviour
{   
    //States the sword operates on with respective 
    private enum SwordState
    {
        ORBITING,
        HOVERING,

        ORBITING_TO,
        MOVING_TO,
        RETURNING_TO,
        RETURNING
    }
    private SwordState state = SwordState.ORBITING;


    //the reference center (the player)
    public GameObject player;
    public GameObject sprite;
    public ParticleSystem bloodParticles;
    public AudioClip hitSound;
    public AudioClip[] swooshSound;
    
    //Orbit Data
        public float rot_speed = 1f;
        //the distance between character and sword
        private float _dist = 2.0f;
        public float dist
        {
            get { return _dist; }
            set { _dist = value; }
        }
        //the relative position on the outer circle
        private float _angle = 0.0f;
        public float angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

    //Movement Data
        public float moveSpeed = 2.0f;
        private Vector3 moveVector;


    private float goalAngle;
    private float onMoveOrbitSpeed;


    // Start is called before the first frame update
    void Start()
    {
        orbit_player_transform();
        onMoveOrbitSpeed = (float)(moveSpeed / _dist); //rearranged from (speed / circuumference)/one rotation together with conversion to angle
        Debug.Log("Computed Orbital Velocity: " + onMoveOrbitSpeed.ToString());
    }

    private Vector3 orbit_pt()
    {
        return orbit_pt(_angle);
    }

    private Vector3 orbit_pt(float angle)
    {
        float sinAngle = (float)Math.Sin(angle);
        float cosAngle = (float)Math.Cos(angle);
        return new Vector3(cosAngle, 0, sinAngle).normalized;
    }

    private (double, double) computeTangentAngles(Vector3 pt)
    {
        Vector3 pos = player.transform.position;

        float x_dist = pt.x - pos.x;
        float z_dist = pt.z - pos.z;
        double vec_dist = Math.Sqrt(x_dist * x_dist + z_dist * z_dist);

        double radius = _dist;

        //relative angle of the first point
        if (radius / vec_dist > 1)
            return (0, Math.Atan2(z_dist, x_dist));
        else if(radius / vec_dist < -1)
            return (Math.PI, Math.Atan2(z_dist, x_dist));
        else
            return (Math.Acos(radius / vec_dist), Math.Atan2(z_dist, x_dist));
    }

    private Vector3 computeNearestOrbitAngle(Vector3 pt, bool post = true)
    {
        //relative angle of the first point
        (double a, double b) = computeTangentAngles(pt);

        Debug.DrawLine(player.transform.position + orbit_pt((float)b) * _dist, player.transform.position);

        //technically there are two soluvations but we always want to approach the next point clockwise
        if (post)
            return orbit_pt((float)(a + b));
        else
            return orbit_pt((float)(b - a));
    }

    //compute the position based on the orbit of the player
    public void orbit_player_transform()
    {
        transform.position = player.transform.position + orbit_pt() * _dist;
    }
    public void prepare_return()
    {
        //return to last orbit point
        sprite.transform.localRotation = Quaternion.AngleAxis(45f, Vector3.up) * Quaternion.AngleAxis(35.264f, Vector3.right);
        transfer_state(SwordState.RETURNING);
    }

    //move object to point
    public void moveTo(Vector3 point)
    {
        RaycastHit hit;
        Vector3 direction = point - transform.position;
        float distance = direction.magnitude;
        direction /= distance;
        float minDistance = Vector3.Dot(player.transform.position - transform.position, direction);
        minDistance = Mathf.Max(0, minDistance);
        minDistance = Mathf.Min(minDistance, distance);
        // 0b1000 is layermask for walls
        if (Physics.Raycast(transform.position + direction * minDistance, direction, out hit, distance - minDistance, 0b1000)) {
            moveVector = direction * (hit.distance - 0.5f + minDistance) + transform.position;
        } else {
            moveVector = point;
        }
        moveVector.y = transform.position.y;
        float angle = Mathf.Atan2(moveVector.z - transform.position.z, moveVector.x - transform.position.x);

        sprite.transform.localRotation = Quaternion.AngleAxis(-angle / Mathf.PI * 180f - 90f, Vector3.up) * Quaternion.AngleAxis(90, Vector3.right);

        if (state == SwordState.HOVERING || state == SwordState.RETURNING_TO)
            transfer_state(SwordState.RETURNING_TO);
        else if (state == SwordState.ORBITING_TO || state == SwordState.ORBITING)
            transfer_state(SwordState.MOVING_TO);

        GetComponent<AudioSource>().PlayOneShot(swooshSound[UnityEngine.Random.Range(0, swooshSound.Length)]);
    }

    //transfer function for the sword state machine
    //down the line will update animation states as well
    private bool transfer_state(in SwordState in_state)
    {
        Debug.Log("Transfering from " + state.ToString() + " -> " + in_state.ToString());
        if (state == SwordState.ORBITING || state == SwordState.ORBITING_TO)
        {
            //accept valid state transform
            if (in_state == SwordState.MOVING_TO)
            {
                state = SwordState.MOVING_TO;
            }
            else
                return false;
        }
        else if (state == SwordState.MOVING_TO)
        {
            //Hovering is a temporary state
            if (in_state == SwordState.HOVERING)
            {
                //Sword returns after half a second
                state = SwordState.HOVERING;
                //Invoke("prepare_return", 1.0f);
            }
            else
                return false;
        }
        else if (state == SwordState.HOVERING)
        {
            //accept valid state transform
            if (in_state == SwordState.RETURNING_TO)
            {
                state = SwordState.RETURNING_TO;
            }
            else if (in_state == SwordState.RETURNING)
            {
                state = SwordState.RETURNING;
            }
            else
                return false;
        }
        else if (state == SwordState.RETURNING_TO)
        {
            //accept valid state transform
            if (in_state == SwordState.ORBITING_TO)
            {
                state = SwordState.ORBITING_TO;
            }
            else if (in_state == SwordState.RETURNING)
            {
                state = SwordState.RETURNING;
            }
            else
                return false;
        }
        else if (state == SwordState.RETURNING)
        {
            //accept valid state transform
            if (in_state == SwordState.ORBITING)
            {
                state = SwordState.ORBITING;
            }
            else if (in_state == SwordState.RETURNING_TO)
            {
                state = SwordState.RETURNING_TO;
            }
            else
                return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SwordState.ORBITING)
        {
            //normalize using distance
            angle += (float)(rot_speed * Time.deltaTime / (2 * Math.PI * _dist));
            //reset after orbit
            if (angle > Math.PI)
                angle -= (float)(2 * Math.PI);

            orbit_player_transform();
        }
        else if (state == SwordState.ORBITING_TO)
        {
            (double a, double b) = computeTangentAngles(moveVector);
            float goalAngle1 = (float)(b - a);
            float goalAngle2 = (float)(b + a);

            Debug.DrawLine(moveVector, player.transform.position + orbit_pt(goalAngle1) * _dist, Color.green, 2.0f);
            Debug.DrawLine(moveVector, player.transform.position + orbit_pt(goalAngle2) * _dist, Color.red, 2.0f);

            goalAngle = goalAngle1;
            float deltaAngle = Math.Abs(goalAngle - angle);
            deltaAngle = (float)Math.Min(deltaAngle, (2 * Math.PI) - deltaAngle);

            //check if we would overshoot our point
            if (deltaAngle <= onMoveOrbitSpeed * Time.deltaTime)
            {
                //arrive at point
                transform.position = player.transform.position + orbit_pt(goalAngle) * _dist;

                //reset after orbit but only do that once we arrived to not make calculations more complicated

                //transfer state to arrival, this should be handled separately??
                transfer_state(SwordState.MOVING_TO);
            }
            else
            {
                //normalize using distance
                angle += onMoveOrbitSpeed * Time.deltaTime;
                transform.position = player.transform.position + orbit_pt(angle) * _dist;
            }
            if (angle > Math.PI)
                angle -= (float)(2 * Math.PI);
            

        }
        else if (state == SwordState.MOVING_TO)
        {
            Vector3 d = moveVector - transform.position;
            //should this state be in the state transfer?
            if (d.magnitude <= moveSpeed * Time.deltaTime)
            {
                //arrive at point
                transform.position = moveVector;

                //transfer state to arrival, this should be handled separately??
                transfer_state(SwordState.HOVERING);
            }
            else
            {
                Vector3 dir = d.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
            }
        }
        else if (state == SwordState.RETURNING)
        {
            Vector3 mv = player.transform.position + computeNearestOrbitAngle(transform.position) * _dist;

            Vector3 d = mv - transform.position;
            //should this state be in the state transfer?
            if (d.magnitude <= moveSpeed * Time.deltaTime)
            {
                //arrive at point
                transform.position += d;

                //set angle to be in the correct value
                angle = (float)Math.Atan2(transform.position.z - player.transform.position.z, transform.position.x - player.transform.position.x);

                //transfer state to arrival, this should be handled separately??
                transfer_state(SwordState.ORBITING);
            }
            else
            {
                Vector3 dir = d.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
            }
        }
        else if (state == SwordState.RETURNING_TO)
        {
            Vector3 mv = player.transform.position + computeNearestOrbitAngle(transform.position) * _dist;

            // Debug.Log(computeNearestOrbitAngle(transform.position));

            Debug.DrawLine(transform.position, mv, Color.green);

            //set angle to be in the correct value
            angle = (float)Math.Atan2(transform.position.z - player.transform.position.z, transform.position.x - player.transform.position.x);

            Vector3 d = mv - transform.position;

            //should this state be in the state transfer?
            if (d.magnitude <= moveSpeed * Time.deltaTime)
            {
                //arrive at point
                transform.position = mv;

                //transfer state to arrival, this should be handled separately??
                transfer_state(SwordState.ORBITING_TO);
            }
            else
            {
                Vector3 dir = d.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        BaseMob mob = collision.gameObject.GetComponent<BaseMob>();
        if (mob != null)
        {
            bloodParticles.Play();
            GetComponent<AudioSource>().PlayOneShot(hitSound);
            mob.OnReceiveDamage();
        }
    }
    
}
