using UnityEngine;

public class AntiRotation : MonoBehaviour {

    public Vector3 rotation;

    void Update() {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
