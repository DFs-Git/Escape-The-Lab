using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopmentMode : MonoBehaviour
{
    public void ButtonClick()
    {
        if (PlayerPrefs.GetInt("DevelopmentMode") == 0)
            PlayerPrefs.SetInt("DevelopmentMode", 1);
        else
            PlayerPrefs.SetInt("DevelopmentMode", 0);

        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("DevelopmentMode"));
    }
}
