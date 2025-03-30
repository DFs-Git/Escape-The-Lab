using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public LevelLoader Loader;

    public TMP_Text Title;
    public TMP_Text TaskDescription;

    void Awake()
    {
        
    }

    void Start()
    {
        ;
        string titleContext = (char)(LevelLoader.Instance.Chap + '0') + "-" + 
            (char)(LevelLoader.Instance.Top + '0') + " " + LevelLoader.Instance.TitleText;
        Title.text = titleContext;
        TaskDescription.text = LevelLoader.Instance.TaskDescription;
    }
}
