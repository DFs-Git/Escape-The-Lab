using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CL = ChemicalLoader;
using UnityEngine.SceneManagement;

public class CommitController : MonoBehaviour
{
    public LevelLoader Loader;
    public Flowchart flowchart;

    public TMP_Text CommitDescription;

    // 提交的物质
    public List<GameObject> CommitChemicals;

    void Awake()
    {
        CL.LoadChemicals();
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
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

    public void Success()
    {
        if (Loader.level.chapter == PlayerPrefs.GetInt("chapter") &&
            Loader.level.topic == PlayerPrefs.GetInt("topic"))
        {
            if (Loader.level.chapter != 6 || Loader.level.topic != 10)
            {
                Debug.Log("not Last");
                flowchart.ExecuteBlock("Pass");
            }
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
                Debug.Log("Last");
                flowchart.ExecuteBlock("After");
            }
        }
    }

    public void Failed()
    {
        flowchart.ExecuteBlock("NotPass");
    }
}
