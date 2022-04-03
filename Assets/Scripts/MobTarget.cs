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

    private MobSpawner diff;

    void Start()
    {
        diff = GetComponent<MobSpawner>();
        scoreObj.SetScore(difficultyOffset);
    }

    // Update is called once per frame
    void Update()
    {
        Hurt(hurtCurve.Evaluate(diff.difficulty) * Time.deltaTime);
        scoreObj.addScore(difficultyMultiplier * Time.deltaTime);
    }

    public void Hurt(float damage) {
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
    }
}
