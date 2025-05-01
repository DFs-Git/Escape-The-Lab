using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameList : MonoBehaviour
{
    public Mask mask;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    public void Back()
    {
        StartCoroutine(mask.MaskFadeIn("Settings"));
    }

    public void GoNameList()
    {
        StartCoroutine(mask.MaskFadeIn("NameList"));
    }
}
