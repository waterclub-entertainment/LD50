using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTarget : MonoBehaviour
{
    public int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Hit detected");
        if (hit.gameObject.tag == "Mob")
        {
            health -= 1;

            if (health <= 0)
            {
                //To Be Changed
                Debug.Log("YOU DIED");
            }
        }
    }
}
