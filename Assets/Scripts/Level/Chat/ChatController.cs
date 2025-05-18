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

        // 读取对话json文件
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
            DontDestroyOnLoad(gameObject); // 确保该对象在切换场景时不会被销毁
        }
        else
        {
            Destroy(gameObject); // 销毁重复的实例
        }
    }
}
