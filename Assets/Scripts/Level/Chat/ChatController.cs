using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter.Interop.LuaStateInterop;

[Serializable]
public class Dialog
{
    public List<List<string>> dialogs;
}

// 现在类 ChatController 相当于一个加载和存放对话信息的容器。
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
            DontDestroyOnLoad(gameObject); // 确保该对象在切换场景时不会被销毁
        }
        else
        {
            Destroy(gameObject); // 销毁重复的实例
        }
    }

    // 加载指定对话
    public void LoadChat(string path)
    {
        TextAsset chatJson = Resources.Load<TextAsset>("Dialogues/" + path);
        dialog = JsonConvert.DeserializeObject<Dialog>(chatJson.text);
    }
}
