using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mask : MonoBehaviour
{
    public Image image;

    public float TuringDuration = 0.7F;

    public bool fadingIn = false;
    public bool fadingOut = false;

    void Start()
    {
        StartCoroutine(MaskFadeOut());
    }

    // Mask渐隐(界面渐显)
    public IEnumerator MaskFadeOut()
    {
        if (!fadingIn)
        {
            while (image.color.a > 0 && !fadingIn)
            {
                fadingOut = true;
                Debug.Log("turing");
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.01F);
                yield return new WaitForSeconds(TuringDuration / 100.0F);
            }

            fadingOut = false;
        }
    }

    // Mask渐显(界面渐隐)跳转其他界面
    public IEnumerator MaskFadeIn(int sceneNumber)
    {
        if (fadingOut)
        {
            StopCoroutine(MaskFadeOut());
            fadingOut = false;
        }
        if (!fadingOut)
        {
            while (image.color.a < 1.0F && !fadingOut)
            {
                fadingIn = true;
                Debug.Log("turing");
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.01F);
                
                yield return new WaitForSeconds(TuringDuration / 100.0F);
            }

            fadingIn = false;
            SceneManager.LoadScene(sceneNumber);
        }
    }
}
