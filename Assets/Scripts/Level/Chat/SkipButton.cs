using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    public Mask mask;
    public ChatBuilder Builder;

    void Awake()
    {
        Builder = Camera.main.gameObject.GetComponent<ChatBuilder>();
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    public void Skip()
    {
        StopCoroutine(Builder.StartDialog());
        
        for (int i = 0; i < mask.transform.childCount; i++)
        {
            Destroy(mask.transform.GetChild(i).gameObject);
        }

        GameObject dia = GameObject.Find("NormalDialogue");
        if (dia != null)
        {
            Destroy(dia);
        }
        GameObject cho = GameObject.Find("Choice");
        if (cho !=  null)
        {
            Destroy(cho);
        }

        mask.image.raycastTarget = false;
        mask.image.color = new Color(mask.image.color.r, mask.image.color.g, mask.image.color.b, 0);
        Destroy(gameObject);
    }
}
