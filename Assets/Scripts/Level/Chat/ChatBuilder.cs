using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBuilder : MonoBehaviour
{
    public ChatController Controller;
    public GameObject Canvas;

    void Start()
    {
        StartCoroutine(StartDialog());
    }

    IEnumerator StartDialog()
    {
        foreach (var content in Controller.dialog.dialogs)
        {
            // ¶Ô»°
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }
    }
}
