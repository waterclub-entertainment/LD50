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

        MOVING_TO,
        RETURNING
    }
    private SwordState state = SwordState.ORBITING;


    //the reference center (the player)
    public GameObject player;
    
    //Orbit Data
        public float rot_speed = 1f;
        //the distance between character and sword
        private float _dist = 2.0f;
        public float dist
        {
            get { return _dist; }
            set { _dist = value; orbit_player_transform(); }
        }
        //the relative position on the outer circle
        private float _angle = 0.0f;
        public float angle
        {
            get { return _angle; }
            set { _angle = value; orbit_player_transform(); }
        }

    //Movement Data
        public float moveSpeed = 2;
        private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        orbit_player_transform();
    }

    private Vector3 orbit_pt()
    {
        double a = Math.PI * _angle / 180.0;
        float sinAngle = (float)Math.Sin(angle);
        float cosAngle = (float)Math.Cos(angle);
        return new Vector3(cosAngle, 0, sinAngle);
    }

    //compute the position based on the orbit of the player
    public void orbit_player_transform()
    {
        transform.position = player.transform.position + orbit_pt() * _dist;
    }

    private void prepare_return()
    {
        //return to last orbit point
        moveVector = player.transform.position + orbit_pt() * _dist;
        transfer_state(SwordState.RETURNING);
    }

    //move object to point
    public void moveTo(Vector3 point)
    {
        if (state == SwordState.ORBITING)
        {
            moveVector = point;
            transfer_state(SwordState.MOVING_TO);
        }

    }

    //transfer function for the sword state machine
    //down the line will update animation states as well
    private bool transfer_state(in SwordState in_state)
    {
        Debug.Log("Transfering from " + state.ToString() + " -> " + in_state.ToString());
        if (state == SwordState.ORBITING)
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
                Invoke("prepare_return", 0.5f);
            }
            else
                return false;
        }
        else if (state == SwordState.HOVERING)
        {
            //accept valid state transform
            if (in_state == SwordState.RETURNING)
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
        }
        else if (state == SwordState.MOVING_TO)
        {
            Vector3 d = moveVector - transform.position;
            //should this state be in the state transfer?
            if (d.magnitude <= moveSpeed * Time.deltaTime)
            {
                //arrive at point
                transform.position += d;

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
            moveVector = player.transform.position + orbit_pt() * _dist;

            Vector3 d = moveVector - transform.position;
            //should this state be in the state transfer?
            if (d.magnitude <= moveSpeed * Time.deltaTime)
            {
                //arrive at point
                transform.position += d;

                //transfer state to arrival, this should be handled separately??
                transfer_state(SwordState.ORBITING);
            }
            else
            {
                Vector3 dir = d.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
            }
        }
    }
}