using UnityEngine;
using UnityEngine.SceneManagement;

public class MobTarget : MonoBehaviour
{
    public float health = 10f;
    public float mobCollisionDamage = 1f;

    public HighscoreData scoreObj;
    public float difficultyMultiplier = 1.0f;
    public float difficultyOffset = 0.0f;

    public AnimationCurve hurtCurve;
    public UIController uiController;

    // "That's the cool part, you don't"
    public bool invincible = false;
    public AudioClip bloodOrbSound;
    public AudioClip hurtSound;

    private MobSpawner diff;

    void Start()
    {
        diff = GetComponent<MobSpawner>();
        scoreObj.SetScore(difficultyOffset);
    }

    // Update is called once per frame
    void Update()
    {
        Hurt(hurtCurve.Evaluate(diff.difficulty) * Time.deltaTime, false);
        scoreObj.addScore(difficultyMultiplier * Time.deltaTime);
        if (uiController != null) {
            uiController.SetHealthLevel(health / 10f);
        }
    }

    public void Hurt(float damage, bool byMob = true) {
        if (invincible && byMob) {
            return;
        }

        if (byMob) {
            GetComponent<AudioSource>().PlayOneShot(hurtSound);
        }
        health -= damage;

        if (health <= 0)
        {
            SceneManager.LoadScene("Scenes/GameOver");
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Mob")
        {
            Hurt(mobCollisionDamage);
        }
        else if (hit.gameObject.tag == "BloodOrb")
        {
            //Handle Blood Orb
            GetComponent<AudioSource>().PlayOneShot(bloodOrbSound);
            Destroy(hit.gameObject);
            health += 1;
        }
    }
}
