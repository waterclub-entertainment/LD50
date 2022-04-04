using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class JingleController : MonoBehaviour {

    public AudioClip[] jingles;

    void Start() {
        GetComponent<AudioSource>().PlayOneShot(jingles[Random.Range(0, jingles.Length)]);
        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Scenes/Main");
    }

}
