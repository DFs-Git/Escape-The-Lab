using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Level
{
    public int chapter, topic; // 当前关卡的章节和课题
    public string title; // 当前关卡的标题
    public string task_description; // 当前关卡的任务描述
    public List<string> tips; // 当前关卡的提示
    public List<int> offered; // 当前关卡的提供物质
    public List<int> commit; // 当前关卡的提交物质
    public List<int> reaction_condition; // 当前关卡的反应条件
}

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public Level level;

    void Awake()
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

    // 加载某个关卡
    public void LoadLevel(int chapter, int topic)
    {
        // 读取相关关卡的json文件，并存入
        TextAsset jsonLevel = Resources.Load<TextAsset>("Levels/level" + chapter.ToString() + "-" + topic.ToString());
        level = JsonConvert.DeserializeObject<Level>(jsonLevel.text);

        Debug.Log(level.chapter.ToString() + "-" + level.topic.ToString());
        Debug.Log(level.title);
        Debug.Log(level.task_description);
        Debug.Log(level.tips);
        Debug.Log(level.offered);
        Debug.Log(level.commit.Count);
        Debug.Log(level.reaction_condition);

        // 跳转到Level场景
        SceneManager.LoadScene(4);
    }
}
