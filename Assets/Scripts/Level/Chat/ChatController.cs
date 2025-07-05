using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class Dialog
{
    public List<List<string>> dialogs;
}

public class ChatController : MonoBehaviour
{
    public static ChatController Instance;

    public LevelLoader Loader;

    public Dialog dialog;

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void Start()
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
}
