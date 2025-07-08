using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSkip : MonoBehaviour
{
    public ChatBuilder Builder;

    public void SkipDialog()
    {
        Builder = GameObject.Find("ChatBuilder").GetComponent<ChatBuilder>();
        Builder.EndDialog();
    }
}
