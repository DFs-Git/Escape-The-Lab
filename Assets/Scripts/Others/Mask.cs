using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mask : MonoBehaviour
{
    public Image image;

    public float TuringDuration = 1.5F;

    void Start()
    {
        StartCoroutine(Turn());
    }

    // 协程用于控制Mask的透明度
    IEnumerator Turn()
    {
        while (image.color.a > 0)
        {
            Debug.Log("turing");
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.01F);
            yield return new WaitForSeconds(TuringDuration / 100.0F);
        }
    }
}
