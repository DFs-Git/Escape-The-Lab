using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatButton : MonoBehaviour
{
    public ChoiceCollector collector;

    public int jump;

    void Awake()
    {
        collector = GameObject.Find("Choice").GetComponent<ChoiceCollector>();
    }

    public void Click()
    {
        collector.ChoiceJump = jump;
    }
}
