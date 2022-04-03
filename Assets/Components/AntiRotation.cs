using UnityEngine;

public class AntiRotation : MonoBehaviour {

    public Vector3 rotation;

    void LateUpdate() {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
