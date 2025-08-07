using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherButtonEvents : MonoBehaviour
{
    public Mask mask;

    void Start()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    public void BackToTitle()
    {
        StartCoroutine(mask.MaskFadeIn("Start"));
    }

    public void ResetSaves()
    {
        PlayerPrefs.SetInt("chapter", 0);
        PlayerPrefs.SetInt("topic", 0);
        PlayerPrefs.Save();

        StartCoroutine(mask.MaskFadeIn("LevelChoose"));
    }
}
