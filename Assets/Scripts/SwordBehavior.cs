using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordBehavior : MonoBehaviour
{   
    //States the sword operates on with respective 
    private enum SwordState
    {
        ORBITING = 0,
        HOVERING = 1,

        MOVING_TO = 2,
        RETURNING = 3,
        MOVING = 2 //this  is the bitmask for moving or not moving
    }
    private SwordState state = SwordState.ORBITING;


    //the reference center (the player)
    public GameObject player;
    
    //Orbit Data
        public float rot_speed = 0.01f;
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
        private float moveSpeed;
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
        moveVector = orbit_pt();
        transfer_state(SwordState.MOVING);
    }

    //move object to point
    public void moveTo(Vector3 point)
    {
        if (0 < (state & SwordState.MOVING))
        {
            moveVector = point;
            transfer_state(SwordState.MOVING_TO);
        }

    }

    //transfer function for the sword state machine
    //down the line will update animation states as well
    private bool transfer_state(in SwordState in_state)
    {
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
                Invoke("prepare_return", 0.5f);
            }
            //accept valid state transform
            else if (in_state == SwordState.ORBITING)
            {
                state = SwordState.ORBITING;
            }
            else
                return false;
        }
        else if (state == SwordState.HOVERING)
        {
            //accept valid state transform
            if (in_state == SwordState.MOVING_TO)
            {
                state = SwordState.MOVING_TO;
            }
            else
                return false;
        }
        //this accounts for returning
        else
            return false;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SwordState.ORBITING)
        {
            //normalize using distance
            angle += (float) (rot_speed / (2 * Math.PI * _dist));
        }
        else if ((state & SwordState.MOVING) != 0)
        {
            Vector3 d = transform.position - moveVector;
            //should this state be in the state transfer?
            if (d.magnitude <= moveSpeed)
            {
                transform.position += d;

                if (state == SwordState.MOVING_TO)
                    transfer_state(SwordState.HOVERING);
                else if (state == SwordState.RETURNING)
                    transfer_state(SwordState.ORBITING);
            }
            else
            {
                Vector3 dir = d.normalized;
                transform.position += dir * moveSpeed;
            }
        }
        else if (state == SwordState.HOVERING)
        {
            //Nothing yet really
        }
    }
}
