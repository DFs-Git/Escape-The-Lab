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
        // ������������
        // ����ı�浵�ؿ�
        // 2������������Ӣ�İ�Ƕ��ŷָ����ֱ��ʾ�½ڡ�����
        // ��Ӣ�İ�Ƕ��Ž���
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
