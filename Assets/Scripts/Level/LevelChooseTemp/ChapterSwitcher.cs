using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSwitcher : MonoBehaviour
{
    public ChapterController Controller;
    public LevelButtonProcessor Processor;

    public Button PageUp;
    public Button PageDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PageUpEvent()
    {
        Controller.LoadChapter(Controller.CurruentChapter - 1);
    }

    public void PageDownEvent()
    {
        Controller.LoadChapter(Controller.CurruentChapter + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.CurruentChapter == 0)
        {
            PageUp.interactable = false;
        }
        else
        {
            PageUp.interactable = true;
        }

        if (Controller.CurruentChapter == PlayerPrefs.GetInt("chapter") ||
            Controller.CurruentChapter == Processor.Structure.chapterCount - 1)
        {
            PageDown.interactable = false;
        }
        else
        {
            PageDown.interactable = true;
        }
    }
}
