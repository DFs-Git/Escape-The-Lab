using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

// 单个反应的信息
public class SingleReaction
{
    public Dictionary<string, int> reactants;
    public int condition;
    public List<List<int>> products;
}

// 反应信息的集合
public class LevelReactions
{
    public Dictionary<string, List<SingleReaction>> reactions;
}

public class Reaction : MonoBehaviour
{
    public LevelLoader Loader;      // 获取关卡信息
    public ReactionPool Pool;       // 获取反应池内容

    public LevelReactions Reactions; // 反应信息

    public TMP_Dropdown ConditionDropdown;

    // 点击反应按钮，开始反应
    public void ReactionEvent()
    {
        // 先获取关卡对应的反应信息
        string path = "Reactions/reaction" + 
            Loader.level.chapter.ToString() + "-" + Loader.level.topic.ToString();
        TextAsset reactions = Resources.Load<TextAsset>(path);
        // 反应信息解析
        Reactions = JsonConvert.DeserializeObject<LevelReactions>(reactions.text);

        // 获取反应池中的物质
        List<GameObject> reactantList = Pool.Chemicals;
        // 保证反应池中有物质
        if (reactantList.Count == 0)
        {
            Debug.Log("反应池中没有物质，无法进行反应");
            return;
        }


    }
}
