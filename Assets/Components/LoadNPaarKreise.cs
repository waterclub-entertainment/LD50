using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class LoadNPaarKreise : MonoBehaviour
{
    public ParticleSystem p1;
    public ParticleSystem p2;
    public ParticleSystem p3;
    public GameObject Crystal;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Wait2(1));
            StartCoroutine(Wait3(1));
        }
        
    }

    private IEnumerator Wait2(int dauer)
    {
        p1.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p2.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p3.gameObject.SetActive(true);
    }
    private IEnumerator Wait3(int dauer)
    {
        while (true)
        {
            Crystal.transform.Rotate(0f, 15f, 0f, Space.Self);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
