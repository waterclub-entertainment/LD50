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
        if (Input.GetKeyDown("space")) {
            animator.SetTrigger("Dash");
            if (sword.canReturn())
                sword.prepare_return();
        }
        if (direction.x < 0) {
            spriteRenderer.flipX = true;
        } else if (direction.x > 0) {
            spriteRenderer.flipX = false;
        }
        if (direction.z < 0) {
            animator.SetFloat("Direction_z", -1);
        } else if (direction.z > 0) {
            animator.SetFloat("Direction_z", 1);
        }
        Vector3 velocity = Quaternion.Euler(0, 45, 0) * direction * movementMultiplier * speed;
        characterController.Move(velocity * Time.deltaTime);


        //Click Handling
        if (Input.GetMouseButtonUp(0))
        {
            if (sword.canMove())
            {
                Plane plane = new Plane(Vector3.up, -1);
                float distance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out distance))
                {
                    Debug.Log(ray.GetPoint(distance));
                    sword.moveTo(ray.GetPoint(distance));
                }
            }
        }
    }
}
