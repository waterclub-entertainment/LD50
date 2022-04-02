using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public float speed = 10.0f;
    public Animator animator;
    public float movementMultiplier = 0.0f;
    public bool canControl = true;
    private Vector3 direction; 
    
    void Start() {
        direction = Vector3.zero;
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            animator.SetTrigger("Dash");
        }
        if (canControl) {
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (movement != Vector3.zero) {
                movement.Normalize();
                direction = movement;
            }
        }
        Vector3 velocity = Quaternion.Euler(0, 45, 0) * direction * movementMultiplier * speed;
        transform.Translate(velocity * Time.deltaTime);
    }
}
