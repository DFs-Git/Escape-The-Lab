using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionsButtonEvents : MonoBehaviour
{
    public Mask mask;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    public void BackEvent()
    {
        StartCoroutine(mask.MaskFadeIn("Start"));
    }
}
