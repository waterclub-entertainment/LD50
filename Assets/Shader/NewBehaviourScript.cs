using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
       [SerializeField] private Material material;
    private float disolveamount;
    private bool isdisolving;
    private void update()
    {
        if (isdisolving = true)
        {
            disolveamount = Mathf.Clamp01(disolveamount + Time.deltaTime);
            material.SetFloat();
        }
    }

}
