using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterController : MonoBehaviour
{
    public int CurruentChapter;

    public LevelButtonProcessor Processor;

    public TMP_Text chapterText;
    public TMP_Text chapterNameText;
    public TMP_Text chapterIntroText;

    // Start is called before the first frame update
    void Start()
    {
        LoadChapter(PlayerPrefs.GetInt("chapter"));
    }

    public void LoadChapter(int chapter)
    {
        CurruentChapter = chapter;
        chapterText.text = chapter.ToString();
        chapterNameText.text = $"《{Processor.Structure.chapterName[chapter]}》";
        chapterIntroText.text = Processor.Structure.chapterIntro[chapter];
        // 加载当前章节的按钮
        Processor.LoadButtons(chapter);
    }

    void Update()
    {
        
    }
}
