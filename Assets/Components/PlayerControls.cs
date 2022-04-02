using UnityEngine;

public class PlayerControls : MonoBehaviour {


    public GameObject swordObject;
    private SwordBehavior sword;

    public float speed = 10.0f;
    public Animator animator;
    public CharacterController characterController;
    public SpriteRenderer spriteRenderer;
    public float movementMultiplier = 0.0f;
    public bool canControl = true;
    private Vector3 direction; 
    
    void Start() {
        direction = Vector3.zero;
        sword = swordObject.GetComponent<SwordBehavior>();
    }

    void Update() {
        //Movement
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


        //Click Handling
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, 0);
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                sword.moveTo(ray.GetPoint(distance));
            }
        }
    }
}
