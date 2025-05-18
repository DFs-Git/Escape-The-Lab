using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

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

        // ��ȡ�Ի�json�ļ�
        TextAsset json = Resources.Load<TextAsset>("Dialogues/dialog" + Loader.level.chapter.ToString() 
            + "-" + Loader.level.topic.ToString());
        dialog = JsonConvert.DeserializeObject<Dialog>(json.text);

        Debug.Log(dialog.dialogs.Count);
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
