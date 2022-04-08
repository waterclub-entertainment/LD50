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
    public AudioClip[] walkSounds;
    public float walkCooldown = 0f;

    private Vector3 direction;

    void Start() {
        direction = Vector3.zero;
        sword = swordObject.GetComponent<SwordBehavior>();
    }

    void Update() {
        walkCooldown -= Time.deltaTime;
        if (walkCooldown <= 0) {
            walkCooldown = 0;
            if (movementMultiplier == 1.0f) {
                walkCooldown = 0.3f;
                GetComponent<AudioSource>().PlayOneShot(walkSounds[Random.Range(0, walkSounds.Length)]);
            }
        }
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
        // Player can't move up or down
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);


        //Click Handling
        if (Input.GetMouseButtonUp(0))
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
