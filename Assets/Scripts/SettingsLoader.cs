using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    void Awake()
    {
        // ���û�д洢���ݵ�PlayerPrefs����洢Ĭ��ֵ
        LoadSettings();
    }

    // �����������õ�settings
    public void LoadSettings()
    {
        /*
        // �����ļ���·��
        string path = "Assets/Resources/settings.json.json";
        string settingsJson;

        // ���·�����ھͶ�
        if (File.Exists(path))
        {
            settingsJson = File.ReadAllText(path);
        }
        else return; // ���߷���һ��������Ϣ��Ĭ��ֵ

        Debug.Log(settingsJson);
        // ����
        saving = JsonUtility.FromJson<Savings>(settingsJson);
        */

        // û�д洢����
        if (!PlayerPrefs.HasKey("lang"))
        {
            // ��Ĭ������
            PlayerPrefs.SetString("lang", "zh-cn");
            PlayerPrefs.SetInt("width", 1920);
            PlayerPrefs.SetInt("height", 1080);
            PlayerPrefs.SetInt("fullscreen", 1);
            PlayerPrefs.SetInt("chapter", 0);
            PlayerPrefs.SetInt("topic", 0);
            // ���浽����
            PlayerPrefs.Save();
        }
    }
}
