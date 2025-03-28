using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// settings.json对应的类
[System.Serializable]
public class Settings
{
    public string lang;
    public int width;
    public int height;
    public bool fullscreen;
}

public class SettingsLoader : MonoBehaviour
{
    // 可以在其他脚本中引用
    public Settings settings;

    void Awake()
    {
        LoadSettings();
    }

    // 加载所有设置到settings
    public void LoadSettings()
    {
        // =设置文件的路径
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
        settings = JsonUtility.FromJson<Settings>(settingsJson);
    }

    // 写入settings.json
    public void WriteSettings()
    {
        string path = "Assets/Resources/settings.json.json";
        // 将settings转换成json格式
        string content = JsonUtility.ToJson(settings);

        // 确保路径存在
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Debug.LogError("未找到settings.json文件！请确认文件位于Resources文件夹。");
            return;
        }

        Debug.Log(content);

        // 写入文件
        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(content);
            }
        }
    }
}
