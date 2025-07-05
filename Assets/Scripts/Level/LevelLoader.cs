using Fungus;               // 用于对话系统的命名空间
using Newtonsoft.Json;      // JSON序列化/反序列化库
using Newtonsoft.Json.Linq; // 用于处理动态JSON对象
using System;               // 基础系统命名空间
using System.Collections.Generic; // 集合类命名空间
using UnityEngine;          // Unity引擎核心命名空间
using CL = ChemicalLoader;  // 化学加载器的别名
using System.Linq;          // LINQ查询扩展
using System.IO;            // 文件IO操作

[Serializable]
public struct Level
{
    /// <summary>
    /// 关卡类型
    /// </summary>
    public int type;

    /// <summary>
    /// 关卡章节
    /// </summary>
    public int chapter;

    /// <summary>
    /// 关卡题目
    /// </summary>
    public int topic;
    /// <summary>
    /// 关卡标题
    /// </summary>
    public string title;

    /// <summary>
    /// 关卡任务描述
    /// </summary>
    public string task_description;

    /// <summary>
    /// 关卡提示
    /// </summary>
    public List<string> tips;

    /// <summary>
    /// 关卡提供的物质
    /// </summary>
    public List<int> offered;
    /// <summary>
    /// 关卡提交的物质
    /// </summary>
    public List<int> commit;

    /// <summary>
    /// 关卡支持的反应条件
    /// </summary>
    public List<int> reaction_condition;

    override public string ToString()
    {
        return "Level(关卡): " + chapter + "-" + topic + "\n" +
               "Title(标题): " + title + "\n" +
               "Task Description(任务描述): " + task_description + "\n" +
               "Tips(提示): " + string.Join(", ", tips) + "\n" +
               "Offered(提供的物质): " + string.Join(", ", offered) + "\n" +
               "Commit(提交的物质): " + string.Join(", ", commit) + "\n" +
               "Reaction Condition(反应条件): " + string.Join(", ", reaction_condition) + "\n";
    }
}


[Serializable]
public struct TempLevel
{
    /// <summary>
    /// 关卡类型
    /// </summary>
    [JsonProperty("type")] // 指定JSON属性映射
    public int type { get; set; } 

    /// <summary>
    /// 关卡章节
    /// </summary>
    [JsonProperty("chapter")] // 指定JSON属性映射
    public int chapter { get; set; } 

    /// <summary>
    /// 关卡题目
    /// </summary>
    [JsonProperty("topic")] // 指定JSON属性映射
    public int topic { get; set; } 
    /// <summary>
    /// 关卡标题
    /// </summary>
    [JsonProperty("title")] // 指定JSON属性映射
    public string title { get; set; }

    /// <summary>
    /// 关卡任务描述
    /// </summary>
    [JsonProperty("task_description")] // 指定JSON属性映射
    public string task_description { get; set; }

    /// <summary>
    /// 关卡提示
    /// </summary>
    [JsonProperty("tips")] // 指定JSON属性映射
    public List<string> tips { get; set; }

    /// <summary>
    /// 关卡提供的物质
    /// </summary>
    [JsonProperty("offered")] // 指定JSON属性映射
    public List<int> offered { get; set; }
    /// <summary>
    /// 关卡提交的物质
    /// </summary>
    [JsonProperty("commit")] // 指定JSON属性映射
    public List<int> commit { get; set; }

    /// <summary>
    /// 关卡支持的反应条件
    /// </summary>
    [JsonProperty("reaction_condition")] // 指定JSON属性映射
    public List<int> reaction_condition { get; set; }
}

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public Level level;

    public Mask mask;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
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

    // 加载某个关卡
    public void LoadLevel(int chapter, int topic)
    {

        // 读取相关关卡的json文件，并存入
        TextAsset jsonLevel = Resources.Load<TextAsset>("Levels/level" + chapter.ToString() + "-" + topic.ToString());
        var tempLevel = JsonConvert.DeserializeObject<TempLevel>(jsonLevel.text);

        //将TempLevel转换为Level
        level = new Level{
            type = Convert.ToInt32(tempLevel.type),
            chapter = Convert.ToInt32(tempLevel.chapter),
            topic = Convert.ToInt32(tempLevel.topic),
            title = tempLevel.title.ToString(),
            task_description = tempLevel.task_description.ToString(),
            tips = tempLevel.tips.Select(x => x.ToString()).ToList(),
            offered = tempLevel.offered.Select(x => Convert.ToInt32(x)).ToList(),
            commit = tempLevel.commit.Select(x => Convert.ToInt32(x)).ToList(),
            reaction_condition = tempLevel.reaction_condition.Select(x => Convert.ToInt32(x)).ToList()
        };

        // 加载对话信息
        // 读取对话json文件
        TextAsset chatJson = Resources.Load<TextAsset>("Dialogues/dialog" + chapter.ToString()
            + "-" + topic.ToString());
        ChatController Chat = GameObject.Find("ChatController").GetComponent<ChatController>();
        Chat.dialog = JsonConvert.DeserializeObject<Dialog>(chatJson.text);

        // Debug.Log(level.ToString());

        mask = GameObject.Find("Mask").GetComponent<Mask>();

        // 跳转到Level场景
        StartCoroutine(mask.MaskFadeIn("Level"));
    }
}
