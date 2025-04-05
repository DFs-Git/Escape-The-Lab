using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseDevelop : MonoBehaviour
{
    public TMP_InputField InputField;

    public TMP_Text dvpText;

    public GameObject Panel;

    void Start()
    {
        if (!PlayerPrefs.HasKey("DevelopmentMode"))
        {
            dvpText.gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("DevelopmentMode") == 0)
        {
            dvpText.gameObject.SetActive(false);
        }
        else
        {
            dvpText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("DevelopmentMode") == 1)
        {
            if (Input.GetKey(KeyCode.Slash))
            {
                Panel.SetActive(true);
            }
        }
    }

    public void Submit()
    {
        // 解析输入内容
        // 这里改变存档关卡
        // 2个正整数，以英文半角逗号分隔，分别表示章节、课题
        // 以英文半角逗号结束
        string value = InputField.text;

        List<int> data = new List<int>();

        int number = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != ',')
            {
                number *= 10;
                number += value[i] - '0';
            }
            else
            {
                data.Add(number);
                number = 0;
            }
        }

        PlayerPrefs.SetInt("chapter", data[0]);
        PlayerPrefs.SetInt("topic", data[1]);

        Panel.SetActive(false);
    }
}
