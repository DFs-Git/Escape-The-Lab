using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    void Awake()
    {
        // 如果没有存储数据到PlayerPrefs，则存储默认值
        LoadSettings();
    }

    // 加载所有设置到settings
    public void LoadSettings()
    {
        /*
        // 设置文件的路径
        string path = "Assets/Resources/settings.json.json";
        string settingsJson;

        // 如果路径存在就读
        if (File.Exists(path))
        {
            settingsJson = File.ReadAllText(path);
        }
        else return; // 或者返回一个错误信息或默认值

        Debug.Log(settingsJson);
        // 解析
        saving = JsonUtility.FromJson<Savings>(settingsJson);
        */

        // 没有存储数据
        if (!PlayerPrefs.HasKey("lang"))
        {
            // 存默认数据
            PlayerPrefs.SetString("lang", "zh-cn");
            PlayerPrefs.SetInt("width", 1920);
            PlayerPrefs.SetInt("height", 1080);
            PlayerPrefs.SetInt("fullscreen", 1);
            PlayerPrefs.SetInt("chapter", 0);
            PlayerPrefs.SetInt("topic", 0);
            // 保存到磁盘
            PlayerPrefs.Save();
        }
    }
}
