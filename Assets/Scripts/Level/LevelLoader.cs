using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public int Chap, Top;
    public string TitleText;
    public string TaskDescription;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ȷ���ö������л�����ʱ���ᱻ����
        }
        else
        {
            Destroy(gameObject); // �����ظ���ʵ��
        }
    }

    // ����ĳ���ؿ�
    public void LoadLevel(int chapter, int topic)
    {
        // ��ȡ��عؿ���json�ļ�
        // (��������)
        Chap = 0;
        Top = 0;
        TitleText = "Turtorial";
        TaskDescription = "Make CO<sub>2</sub> by using the offered chemicals.";

        // ��ת��Level����
        SceneManager.LoadScene(4);
    }
}
