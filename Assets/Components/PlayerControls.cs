using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public float speed = 10.0f;
    public Animator animator;
    public CharacterController characterController;
    public SpriteRenderer spriteRenderer;
    public float movementMultiplier = 0.0f;
    public bool canControl = true;
    private Vector3 direction; 
    
    void Start() {
        direction = Vector3.zero;
    }

    void Update() {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (movement != Vector3.zero) {
            movement.Normalize();
            if (canControl) {
                direction = movement;
            }
            animator.SetBool("Moving", true);
        } else {
            animator.SetBool("Moving", false);
        }
        if (Input.GetMouseButtonDown(1)) {
            animator.SetTrigger("Dash");
        }
        if (direction.x < 0) {
            spriteRenderer.flipX = true;
        } else if (direction.x > 0) {
            spriteRenderer.flipX = false;
        }
        Vector3 velocity = Quaternion.Euler(0, 45, 0) * direction * movementMultiplier * speed;
        characterController.Move(velocity * Time.deltaTime);
    }
}
