using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class LoadLevel : MonoBehaviour
{
    bool LL = false;
    [SerializeField] VisualEffect _MagicCircle;
    void OnTriggerEnter (Collider other)
    {
        StartMC();
        StartCoroutine(WaitBeforeLoad());
    }
    private void StartMC()
    {
        _MagicCircle.Play();
    }
    private IEnumerator WaitBeforeLoad()
    {
        yield return new WaitForSeconds(12);
        SceneManager.LoadScene("Map");
    }
}
