using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Level
{
    public int Chap, Top; // 当前关卡的章节和课题
    public string TitleText; // 当前关卡的标题
    public string TaskDescription; // 当前关卡的任务描述
    public List<string> Dialog; // 当前关卡的对话
    public List<string> Tips; // 当前关卡的提示
    public List<int> Offered; // 当前关卡的提供物质
    public List<int> Commit; // 当前关卡的提交物质
    public List<int> ReactionCondition; // 当前关卡的反应条件
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
        // 读取相关关卡的json文件，并存入level对象
        TextAsset jsonLevel = Resources.Load<TextAsset>("Levels/level" + chapter.ToString() + "-" + topic.ToString());
        level = JsonConvert.DeserializeObject<Level>(jsonLevel.text);

        // 跳转到Level场景
        SceneManager.LoadScene(4);
    }
}
