using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineUIControllerGalaxyMaterials : MonoBehaviour
{

    public Transform prefabHolder;
    public CanvasGroup canvasGroup;

    private Transform[] prefabs;
    private List<Transform> lt;
    private int activeNumber = 0;

    private void Start()
    {
        lt = new List<Transform>();
        prefabs = prefabHolder.GetComponentsInChildren<Transform>(true);

        foreach (Transform tran in prefabs)
        {
            if (tran.parent == prefabHolder)
            {
                lt.Add(tran);
            }
        }

        prefabs = lt.ToArray();
        EnableActive();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            canvasGroup.alpha = 1f - canvasGroup.alpha;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeEffect(true);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeEffect(false);
        }
    }

    // Turn On active VFX Prefab
    public void EnableActive()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (i == activeNumber)
            {
                prefabs[i].gameObject.SetActive(true);
            }
            else
            {
                prefabs[i].gameObject.SetActive(false);
            }
        }
    }

    // Change active VFX
    public void ChangeEffect(bool bo)
    {
        if (bo == true)
        {
            activeNumber++;
            if (activeNumber == prefabs.Length)
            {
                activeNumber = 0;
            }
        }
        else
        {
            activeNumber--;
            if (activeNumber == -1)
            {
                activeNumber = prefabs.Length - 1;
            }
        }

        EnableActive();
    }
}
