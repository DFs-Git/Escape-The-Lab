using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopMode : MonoBehaviour
{
    public GameObject Panel;
    public GameObject devText;

    public TMP_Dropdown chapterOP;
    public TMP_Dropdown topicOP;

    public LevelButtonProcessor Processor;
    public Mask mask;

    void Start()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();

        if (PlayerPrefs.GetInt("DevelopmentMode") == 1)
        {
            devText.SetActive(true);
        }
        else
        {
            devText.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            if (PlayerPrefs.GetInt("DevelopmentMode") == 1)
                OpenPanel();
        }
    }

    public void OpenPanel()
    {
        Panel.SetActive(true);

        int chapterCount = Processor.Structure.chapterCount;
        chapterOP.ClearOptions();

        for (int i = 0; i < chapterCount; i++)
        {
            chapterOP.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }
    }

    public void Apply()
    {
        int chapterIndex = chapterOP.value;
        int topicIndex = topicOP.value;
        if (chapterIndex != 0 && topicIndex == Processor.Structure.topicCount[chapterIndex] - 1) // X
        {
            topicIndex = 10; // Last topic
        }
        PlayerPrefs.SetInt("chapter", chapterIndex);
        PlayerPrefs.SetInt("topic", chapterIndex == 0 ? 0 : topicIndex + 1);
        PlayerPrefs.Save();

        ClosePanel();
        StartCoroutine(mask.MaskFadeIn("LevelChoose"));
    }

    public void ClosePanel()
    {
        Panel.SetActive(false);
    }

    public void UpdateTopicOP()
    {
        if (Panel.activeInHierarchy)
        {
            topicOP.ClearOptions();
            int chapterIndex = chapterOP.value;
            int topicCount = Processor.Structure.topicCount[chapterIndex];
            for (int i = 1; i <= topicCount; i++)
            {
                if (i == topicCount) topicOP.options.Add(new TMP_Dropdown.OptionData("X"));
                else topicOP.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
            }

            topicOP.value = 0;
        }
    }
}
