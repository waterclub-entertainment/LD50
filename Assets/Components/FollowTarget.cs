using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public GameObject target;
    public float maxSpeed = 9f;
    public float halfLife = 0.1f;

    void LateUpdate() {
        Vector3 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        if (distance != 0) {
            direction /= distance;
            float moveDistance = Mathf.Min(distance, Mathf.Max(maxSpeed * Time.deltaTime, (1 - Mathf.Pow(0.5f, Time.deltaTime / halfLife)) * distance));
            transform.Translate(moveDistance * direction);
        }
    }
}
