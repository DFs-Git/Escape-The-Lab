using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopmentMode : MonoBehaviour
{
    public GameObject keyPanel;
    public TMP_InputField KeyInput;

    public void ButtonClick()
    {
        keyPanel.SetActive(true);
        KeyInput.text = "";
        /*
        if (PlayerPrefs.GetInt("DevelopmentMode") == 0)
            PlayerPrefs.SetInt("DevelopmentMode", 1);
        else
            PlayerPrefs.SetInt("DevelopmentMode", 0);

        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("DevelopmentMode"));
        */
    }
}
