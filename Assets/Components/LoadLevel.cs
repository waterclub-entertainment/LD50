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
        
        StartCoroutine(Wait(1));
    }
    private IEnumerator Wait(int dauer)
    {
        p1.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p2.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p3.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p4.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p5.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p6.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p7.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p8.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p9.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p10.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p11.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p12.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        p13.gameObject.SetActive(true);
        yield return new WaitForSeconds(dauer);
        SceneManager.LoadScene("Map");

    }
}
