using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// settings.json��Ӧ����
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
    // �����������ű�������
    public Settings settings;

    void Awake()
    {
        LoadSettings();
    }

    // �����������õ�settings
    public void LoadSettings()
    {
        // =�����ļ���·��
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
        settings = JsonUtility.FromJson<Settings>(settingsJson);
    }

    // д��settings.json
    public void WriteSettings()
    {
        string path = "Assets/Resources/settings.json.json";
        // ��settingsת����json��ʽ
        string content = JsonUtility.ToJson(settings);

        // ȷ��·������
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Debug.LogError("δ�ҵ�settings.json�ļ�����ȷ���ļ�λ��Resources�ļ��С�");
            return;
        }

        Debug.Log(content);

        // д���ļ�
        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(content);
            }
        }
    }
}
