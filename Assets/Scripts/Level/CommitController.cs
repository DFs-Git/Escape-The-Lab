using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CL = ChemicalLoader;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class CommitController : MonoBehaviour
{
    public LevelLoader Loader;
    // public Flowchart flowchart;
    public ChatController chatController;
    public ChatBuilder chatBuilder;

    public TMP_Text CommitDescription;

    // 提交的物质
    public List<GameObject> CommitChemicals;

    void Awake()
    {
        CL.LoadChemicals();
        // 获取组件
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        chatController = GameObject.Find("ChatController").GetComponent<ChatController>();
        chatBuilder = GameObject.Find("ChatBuilder").GetComponent<ChatBuilder>();
    }

    void Start()
    {
        // 改变CommitDescription的字
        CommitDescription.text = "提交";
        for (int i = 0; i < Loader.level.commit.Count; i += 2)
        {
            Chemical che = CL.FindChemicals(Loader.level.commit[i])[0];
            CommitDescription.text += che.Formula;
            CommitDescription.text += "*" + Loader.level.commit[i + 1].ToString();
            // 最后一个不生成逗号
            if (i + 2 < Loader.level.commit.Count) CommitDescription.text += ",";
        }
        CommitDescription.text += "。";
    }

    // 点击提交按钮
    public void Commit()
    {
        // 检查每一个提交的物质
        // 注意：数量可以多，不可以少
        CommitChemicals = GameObject.Find("CommitArea").GetComponent<CommitController>().CommitChemicals;

        Debug.Log("Count Error " + CommitChemicals.Count.ToString());
        Debug.Log("Count Error " + (Loader.level.commit.Count / 2));
        if (CommitChemicals.Count != (Loader.level.commit.Count / 2))
        {
            Debug.Log("Count Error");
            Failed();
            return;
        }

        for (int i = 0; i < Loader.level.commit.Count; i += 2)
        {
            Chemical chem = CL.FindChemicals(Loader.level.commit[i])[0];
            Debug.Log(chem.ID.ToString() + " " + Loader.level.commit[i + 1].ToString());
            bool found = false;
            foreach (GameObject che in CommitChemicals)
            {
                
                if (che.GetComponent<Chemicals>().ChemicalInclude.ID == chem.ID &&
                    che.GetComponent<Chemicals>().Count >= Loader.level.commit[i + 1])
                {
                    
                    found = true; break;
                }
            }
            
            if (!found)
            {
                Debug.Log("Kind Error");
                Failed();
                return;
            }
        }

        Success();
    }

    // 判断资源是否存在
    public bool ResourceExists(string resourceName)
    {
        return Resources.Load<UnityEngine.Object>(resourceName) != null;
    }

    public void Success()
    {
        // 判断是不是通关了正在进行中的关卡，如果不是就不做任何事
        if (Loader.level.chapter == PlayerPrefs.GetInt("chapter") &&
            Loader.level.topic == PlayerPrefs.GetInt("topic"))
        {
            // Debug.Log("not Last");
            // flowchart.ExecuteBlock("Pass");
            GetComponent<Button>().interactable = false;

            chatController.LoadChat("Pass");
            chatBuilder.BuilderShowDialog(() =>
            {
                // 可能存在通关后的特殊对话
                // dialogx-x-after 这样式的
                if (ResourceExists("Dialogues/dialog" + Loader.level.chapter.ToString() + "-" + Loader.level.topic.ToString() + "-after"))
                {
                    // 加载它
                    chatController.LoadChat("dialog" + Loader.level.chapter.ToString() + "-" + Loader.level.topic.ToString() + "-after");
                    chatBuilder.BuilderShowDialog(() => {
                        if (Loader.mask == null)
                            Loader.mask = GameObject.Find("Mask").GetComponent<Mask>();
                        StartCoroutine(Loader.mask.MaskFadeIn("ChooseLevel"));
                    });
                }
                else
                {
                    // 没有特殊对话，直接返回关卡选择界面
                    if (Loader.mask == null)
                        Loader.mask = GameObject.Find("Mask").GetComponent<Mask>();
                    StartCoroutine(Loader.mask.MaskFadeIn("ChooseLevel"));
                }
            });

            /// 处理关卡进度
            //初始章节
            if (PlayerPrefs.GetInt("chapter") == 0)
            {
                PlayerPrefs.SetInt("chapter", 1);
                PlayerPrefs.SetInt("topic", 1);
            }
            //每chapter倒数第二topic
            else if ((PlayerPrefs.GetInt("chapter") == 1 &&
                PlayerPrefs.GetInt("topic") == 1) ||
                (PlayerPrefs.GetInt("chapter") == 2 &&
                PlayerPrefs.GetInt("topic") == 3) ||
                (PlayerPrefs.GetInt("chapter") == 3 &&
                PlayerPrefs.GetInt("topic") == 2) ||
                (PlayerPrefs.GetInt("chapter") == 4 &&
                PlayerPrefs.GetInt("topic") == 3) ||
                (PlayerPrefs.GetInt("chapter") == 5 &&
                PlayerPrefs.GetInt("topic") == 2) ||
                (PlayerPrefs.GetInt("chapter") == 6 &&
                PlayerPrefs.GetInt("topic") == 6))
            {
                PlayerPrefs.SetInt("topic", 10);
            }
            //每chapter倒数最后一topic
            else if (PlayerPrefs.GetInt("chapter") < 6 && PlayerPrefs.GetInt("topic") == 10)
            {
                PlayerPrefs.SetInt("chapter", Loader.level.chapter + 1);
                PlayerPrefs.SetInt("topic", 1);
            }
            //其余
            else if(PlayerPrefs.GetInt("chapter") <= 6 && PlayerPrefs.GetInt("topic")!=10)
            {
                PlayerPrefs.SetInt("topic", Loader.level.topic +1);
            }
            // 终章直接改成课题11
            else if (PlayerPrefs.GetInt("chapter") == 6 && PlayerPrefs.GetInt("topic") == 10)
            {
                PlayerPrefs.SetInt("topic", 11);
            }
        }
    }

    // 提交错误
    public void Failed()
    {
        // flowchart.ExecuteBlock("NotPass");
        // 禁用 button
        GetComponent<Button>().interactable = false;
        chatController.LoadChat("NotPass");
        chatBuilder.BuilderShowDialog(() => {
            // 恢复 button
            GetComponent<Button>().interactable = true;
        });
    }
}
