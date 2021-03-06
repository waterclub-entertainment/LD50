using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileBehavior : MonoBehaviour
{
    private GameObject player;
    public float angularSpeed = 4.0f;
    public float speed = 8.0f;
    public int damagePerHit = 1;
    public float liveTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void turnTowardsPlayer()
    {
        Vector3 d = (player.transform.position - transform.position).normalized;
        double angle = Math.Atan2(transform.forward.z - d.z, transform.forward.x - d.x);
        float mult = 1.0f;
        //rotate faster if behind.
        if (Math.Abs(angle) > Math.PI / 2)
            mult = 2.0f;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, d, angularSpeed * Time.deltaTime * mult, 0.0f);
        newDir.y = 0;
        transform.forward = newDir.normalized;
    }
    public void moveTowardsPlayer()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        liveTime -= Time.deltaTime;
        if (liveTime <= 0) {
            Destroy(gameObject);
            return;
        }
        turnTowardsPlayer();
        moveTowardsPlayer();
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit by Projectile");
            Destroy(gameObject);

            MobTarget tgt = player.GetComponent<MobTarget>();

            tgt.Hurt(damagePerHit);
        }
        else if (other.gameObject.name == "Sword" || other.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
}
