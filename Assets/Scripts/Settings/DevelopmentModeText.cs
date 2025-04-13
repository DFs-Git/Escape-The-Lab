using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopmentModeText : MonoBehaviour
{
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("DevelopmentMode"))
        {
            if (PlayerPrefs.GetInt("DevelopmentMode") == 1)
                text.text = "开发者模式已开启。";
            else
                text.text = "";
        }
    }
}
