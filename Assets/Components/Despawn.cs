using UnityEngine;

public class Despawn : MonoBehaviour {

    public GameObject disable;
    public float lifeTime = 10f;

    void Update() {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0) {
            Destroy(gameObject);
            return;
        }

        if (disable != null && lifeTime <= 5f) {
            disable.SetActive(Mathf.Repeat(lifeTime * 2, 1f) < 0.9f);
        }
    }

}
