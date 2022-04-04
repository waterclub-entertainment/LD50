using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class LoadLevel : MonoBehaviour
{
    public ParticleSystem p1;
    public ParticleSystem p2;
    public ParticleSystem p3;
    public ParticleSystem p4;
    public ParticleSystem p5;
    public ParticleSystem p6;
    public ParticleSystem p7;
    public ParticleSystem p8;
    public ParticleSystem p9;
    public ParticleSystem p10;
    public ParticleSystem p11;
    public ParticleSystem p12;
    public ParticleSystem p13;
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player") {
            StartCoroutine(Wait(1));
        }
    }
    private IEnumerator Wait(int dauer)
    {
        p1.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p2.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p3.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        p4.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p5.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p6.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        p7.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p8.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p9.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        p10.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p11.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        p12.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        p13.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Map");

    }
}
