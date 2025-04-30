using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyInputButtons : MonoBehaviour
{
    public GameObject panel;
    public TMP_InputField KeyInput;

    public void Commit()
    {
        // Hash - 69578
        if (KeyInput.text == "MWM_114514")
        {
            // 哈希码正确则开启
            if (PlayerPrefs.GetInt("DevelopmentMode") == 0)
                PlayerPrefs.SetInt("DevelopmentMode", 1);
            else
                PlayerPrefs.SetInt("DevelopmentMode", 0);

            PlayerPrefs.Save();
            Debug.Log(PlayerPrefs.GetInt("DevelopmentMode"));

            panel.SetActive(false);
        }
    }

    public void Cancel()
    {
        panel.SetActive(false);
    }
}
