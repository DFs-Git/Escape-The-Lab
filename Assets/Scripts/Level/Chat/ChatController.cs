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
}
